using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class CreateModel(
    DefaultContext context,
    IStudentParticipationService participationService,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty]
    public Student Student { get; set; } = new()
    {
        Groups = new List<Group>()
    };

    public List<StudentParticipationDto> GroupParticipationData { get; set; } = [];

    public IActionResult OnGet()
    {
        GroupParticipationData = participationService.GetGroupParticipationData(Student);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string[] selectedGroups)
    {
        selectedGroups ??= [];

        var newStudent = new Student();

        participationService.UpdateStudentGroups(selectedGroups, newStudent);

        try
        {
            if (await TryUpdateModelAsync(
                    newStudent,
                    "student",
                    s => s.FirstName, s => s.Prefix, s => s.LastName,
                    s => s.PhoneNumber, s => s.EmailAddress, s => s.Motivation))
            {
                try
                {
                    context.Students.Add(newStudent);
                    await context.SaveChangesAsync();
                    return RedirectToPage("./Details", new { id = newStudent.StudentId });
                }
                catch (Exception ex)
                {
                    logger.LogError("An unexpected error occurred: {Error}", ex.Message);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating student: {Error}", ex.Message);
        }

        GroupParticipationData = participationService.GetGroupParticipationData(newStudent);
        return Page();
    }
}