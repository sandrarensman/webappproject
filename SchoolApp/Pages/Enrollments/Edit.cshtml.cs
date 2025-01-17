using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class EditModel(
    IStudentSelectionService studentSelectionService,
    DefaultContext context,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Enrollment Enrollment { get; set; } = null!;

    public SelectList StudentNameSelectList { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var enrollment = await context.Enrollments
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);

        if (enrollment == null) return NotFound();
        Enrollment = enrollment;

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(id);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var enrollmentToUpdate = await context.Enrollments.FindAsync(id);
        if (enrollmentToUpdate == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(enrollmentToUpdate.StudentId);
            return Page();
        }

        if (await TryUpdateModelAsync(
                enrollmentToUpdate,
                "enrollment",
                e => e.EnrollmentType,
                e => e.StartDate,
                e => e.EndDate, 
                e => e.StudentId))
        {
            try
            {
                await context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id = enrollmentToUpdate.EnrollmentId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EnrollmentExists(enrollmentToUpdate.EnrollmentId))
                {
                    ModelState.AddModelError("", "The enrollment has been removed by another user.");
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

        StudentNameSelectList = await studentSelectionService.GetStudentDropdownListAsync(enrollmentToUpdate.StudentId);
        return Page();
    }
    
    private async Task<bool> EnrollmentExists(int id)
    {
        return await context.Enrollments.AnyAsync(e => e.EnrollmentId == id);
    }
}