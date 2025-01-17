using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Interfaces.Helpers;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class EditModel(
    IStudentSelectionService studentSelectionService,
    IGroupStudentFilterService groupStudentFilterService,
    DefaultContext context,
    IStudentValidator validator,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Group Group { get; set; } = null!;

    [BindProperty] public List<int> AddedStudentIds { get; set; } = [];
    [BindProperty] public List<int> CurrentStudentIds { get; set; } = [];

    public List<Student> AddedStudents { get; set; } = [];
    public List<Student> CurrentStudents { get; set; } = [];
    public List<Student> AvailableStudents { get; set; } = [];

    public SelectList StudentNameSelectList { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var group = await context.Groups
            .Include(g => g.Students
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName))
            .FirstOrDefaultAsync(g => g.GroupId == id);
        if (group == null) return NotFound();

        CurrentStudentIds = group.Students.Select(s => s.StudentId).ToList();
        CurrentStudents = group.Students.ToList();
        
        var filterResult = await groupStudentFilterService.FilterStudentsForGroupAsync(
            AddedStudentIds, CurrentStudentIds, false);

        AddedStudents = filterResult.AddedStudents;
        AvailableStudents = filterResult.AvailableStudents;

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();

        Group = group;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var groupToUpdate = await context.Groups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.GroupId == id);

        if (groupToUpdate == null) return NotFound();
        
        var filterResult = await groupStudentFilterService.FilterStudentsForGroupAsync(
            AddedStudentIds, CurrentStudentIds, true);
        
        if (!ModelState.IsValid)
        {
            CurrentStudents = groupToUpdate.Students.ToList();
            
            AddedStudents = filterResult.AddedStudents;
            AvailableStudents = filterResult.AvailableStudents;
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();

            return Page();
        }
        
        var allStudentIds = CurrentStudentIds.Union(AddedStudentIds).ToList();

        try
        {
            var validStudents = await validator.ValidateStudentsAsync(allStudentIds);
            groupToUpdate.Students = validStudents;
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            
            CurrentStudents = groupToUpdate.Students.ToList();
            
            AddedStudents = filterResult.AddedStudents;
            AvailableStudents = filterResult.AvailableStudents;
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();
            return Page();
        }

        if (await TryUpdateModelAsync(
                groupToUpdate,
                "group",
                g => g.Day,
                g => g.Level,
                g => g.StartTime,
                g => g.EndTime))
        {
            try
            {
                await context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id = groupToUpdate.GroupId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GroupExists(groupToUpdate.GroupId))
                {
                    ModelState.AddModelError("", "The group has been removed by another user.");
                    return RedirectToPage("./Index");
                }

                ModelState.AddModelError("", "A concurrency error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                logger.LogError("An unexpected error occurred: {Error}", ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            }
        }
        
        CurrentStudents = groupToUpdate.Students.ToList();
        
        AddedStudents = filterResult.AddedStudents;
        AvailableStudents = filterResult.AvailableStudents;
        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();
        return Page();
        
    }

    private async Task<bool> GroupExists(int id)
    {
        return await context.Groups.AnyAsync(g => g.GroupId == id);
    }
}