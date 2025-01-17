using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Interfaces.Helpers;
using SchoolApp.Interfaces.Services;

namespace SchoolApp.Pages.Groups;

public class CreateModel(
    IStudentSelectionService studentSelectionService,
    DefaultContext context,
    IStudentValidator validator,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Group Group { get; set; }

    [BindProperty] public List<int> AddedStudentIds { get; set; } = [];
    [BindProperty] public int? AvailableStudentId { get; set; }

    public List<Student> AddedStudents { get; set; } = [];
    public List<Student> AvailableStudents { get; set; } = [];

    public SelectList StudentNameSelectList { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        AvailableStudents = await context.Students
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .ToListAsync();

        if (AddedStudentIds.Count != 0)
        {
            AddedStudents = await context.Students
                .Where(s => AddedStudentIds.Contains(s.StudentId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();
        }

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            AddedStudents = await context.Students.Where(s => AddedStudentIds.Contains(s.StudentId)).ToListAsync();
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();

            return Page();
        }
        
        var newGroup = new Group();

        await CreateGroupStudents(newGroup, AddedStudentIds);

        try
        {
            var validStudents = await validator.ValidateStudentsAsync(AddedStudentIds);
            newGroup.Students = validStudents;
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            AddedStudents = await context.Students.Where(s => AddedStudentIds.Contains(s.StudentId)).ToListAsync();
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();

            return Page();
        }

        if (await TryUpdateModelAsync(
                newGroup,
                "group",
                g => g.GroupId,
                g => g.Day,
                g => g.Level,
                g => g.StartTime,
                g => g.EndTime))
        {
            try
            {
                context.Groups.Add(newGroup);
                await context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id = newGroup.GroupId });
            }
            catch (Exception ex)
            {
                logger.LogError("An unexpected error occurred: {Error}", ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            }
        }

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync();
        return Page();
    }

    private async Task CreateGroupStudents(Group groupToCreate, List<int> addedStudentIds)
    {
        var validStudents = await context.Students
            .Where(s => addedStudentIds.Contains(s.StudentId))
            .ToListAsync();

        if (validStudents.Count != addedStudentIds.Count)
        {
            ModelState.AddModelError("", "One or more selected students are invalid.");
            return;
        }

        groupToCreate.Students = validStudents;
    }
}