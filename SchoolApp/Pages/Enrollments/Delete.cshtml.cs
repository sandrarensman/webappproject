using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class DeleteModel(
    DefaultContext context,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty] public Enrollment Enrollment { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await context.Enrollments
            .Include(e => e.Student)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.EnrollmentId == id);

        if (enrollment == null) return NotFound();

        Enrollment = enrollment;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var enrollment = await context.Enrollments.FindAsync(id);
        if (enrollment == null) return NotFound();

        Enrollment = enrollment;

        try
        {
            context.Enrollments.Remove(Enrollment);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            logger.LogError("Error deleting enrollment: {Error}", ex.Message);
            ModelState.AddModelError("", "An error occurred while deleting the record.");
            return Page();
        }
    }
}