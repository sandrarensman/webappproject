using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class EditModel(DefaultContext context) : StudentNamePageModel
{
    [BindProperty]
    public Group Group { get; set; } = null!;
    
    [BindProperty]
    public List<int> SelectedStudentIds { get; set; } = [];
    
    [BindProperty]
    public int? AvailableStudentId { get; set; }

    public List<Student> SelectedStudents { get; set; } = [];
    public List<Student> AvailableStudents { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || context.Groups == null) return NotFound();

        var group = await context.Groups
            .Include(g => g.Students
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName))
            .FirstOrDefaultAsync(m => m.GroupId == id);
        if (group == null) return NotFound();
        
        SelectedStudentIds = group.Students.Select(s => s.StudentId).ToList();
        SelectedStudents = group.Students.ToList();
        
        AvailableStudents = await context.Students
            .Where(s => !SelectedStudentIds.Contains(s.StudentId))
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .ToListAsync();
        
        PopulateStudentsDropDownList(context, selectedStudent: null);
        
        Group = group;
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var groupToUpdate = await context.Groups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.GroupId == Group.GroupId);

        if (groupToUpdate == null) return NotFound();
        
        groupToUpdate.Day = Group.Day;
        groupToUpdate.StartTime = Group.StartTime;
        groupToUpdate.EndTime = Group.EndTime;
        groupToUpdate.Level = Group.Level;
        
        if (AvailableStudentId.HasValue && !SelectedStudentIds.Contains(AvailableStudentId.Value))
        {
            SelectedStudentIds.Add(AvailableStudentId.Value);
        }
        
        await UpdateGroupStudents(groupToUpdate);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (! await GroupExists(Group.GroupId))
            {
                ModelState.AddModelError("", "The group has been removed by another user.");
                return RedirectToPage("./Index");
            }
        }

        return RedirectToPage("./Details", new { id = groupToUpdate.GroupId });
    }

    private async Task<bool> GroupExists(int id)
    {
        return await context.Groups.AnyAsync(e => e.GroupId == id);
    }
    
    private async Task UpdateGroupStudents(Group groupToUpdate)
    {
        var validStudents = await context.Students
            .Where(s => SelectedStudentIds.Contains(s.StudentId))
            .ToListAsync();

        if (validStudents.Count != SelectedStudentIds.Count)
        {
            ModelState.AddModelError("", "One or more selected students are invalid.");
            return;
        }
        
        groupToUpdate.Students = validStudents;
    }

}