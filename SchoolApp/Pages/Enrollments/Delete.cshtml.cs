using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class DeleteModel(DefaultContext context) : PageModel
{
    private readonly DefaultContext _context = context;

    [BindProperty] public Enrollment Enrollment { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.EnrollmentId == id);

        if (enrollment == null) return NotFound();

        Enrollment = enrollment;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment != null)
        {
            Enrollment = enrollment;
            _context.Enrollments.Remove(Enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}