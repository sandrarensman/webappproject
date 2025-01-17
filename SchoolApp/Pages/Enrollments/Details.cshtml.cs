using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class DetailsModel(DefaultContext context) : PageModel
{
    public Enrollment Enrollment { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var enrollment = await context.Enrollments
            .Include(e => e.Student)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);

        if (enrollment == null) return NotFound();

        Enrollment = enrollment;
        return Page();
    }
}