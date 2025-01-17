using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class CreateModel(
    IStudentSelectionService studentSelectionService,
    DefaultContext context,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Enrollment Enrollment { get; set; }
    [BindProperty] public int? StudentId { get; set; }

    public SelectList StudentNameSelectList { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id != null) StudentId = id;
        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var newEnrollment = new Enrollment();
        
        if (!ModelState.IsValid)
        {
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(newEnrollment.StudentId);
            return Page();
        }

        if (await TryUpdateModelAsync(
                newEnrollment,
                "enrollment",
                e => e.EnrollmentId, 
                e => e.EnrollmentType, 
                e => e.StartDate, 
                e => e.EndDate, 
                e => e.StudentId))
        {
            try
            {
                context.Enrollments.Add(newEnrollment);
                await context.SaveChangesAsync();

                return StudentId.HasValue
                    ? RedirectToPage("/Students/Details", new { id = (int)StudentId })
                    : RedirectToPage("./Details", new { id = newEnrollment.EnrollmentId });
            }
            catch (Exception ex)
            {
                logger.LogError("An unexpected error occurred: {Error}", ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            }
            
        }

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(newEnrollment.StudentId);
        return Page();
    }
}