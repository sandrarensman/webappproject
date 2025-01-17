using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class EditModel(
    DefaultContext context,
    IStudentParticipationService participationService,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Student Student { get; set; } = null!;

    public List<StudentParticipationDto> GroupParticipationData { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Student = await context.Students
            .Include(s => s.Groups)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (Student == null) return NotFound();

        GroupParticipationData = participationService.GetGroupParticipationData(Student);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, string[] selectedGroups)
    {
        var studentToUpdate = await context.Students
            .Include(s => s.Groups)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (studentToUpdate == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (await TryUpdateModelAsync(
                studentToUpdate,
                "student",
                s => s.FirstName,
                s => s.Prefix,
                s => s.LastName,
                s => s.PhoneNumber,
                s => s.EmailAddress,
                s => s.Motivation))
        {
            try
            {
                participationService.UpdateStudentGroups(selectedGroups, studentToUpdate);
                await context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id = studentToUpdate.StudentId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExists(studentToUpdate.StudentId))
                {
                    ModelState.AddModelError("", "The student has been removed by another user.");
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
        
        GroupParticipationData = participationService.GetGroupParticipationData(studentToUpdate);
        return Page();
    }
    
    private async Task<bool> StudentExists(int id)
    {
        return await context.Students.AnyAsync(s => s.StudentId == id);
    }
}