using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class EditModel(DefaultContext context) : StudentNamePageModel
{
    private readonly DefaultContext _context = context;

    [BindProperty] public Enrollment Enrollment { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);

        if (enrollment == null) return NotFound();
        Enrollment = enrollment;

        PopulateStudentsDropDownList(_context, Enrollment.StudentId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var enrollmentToUpdate = await _context.Enrollments.FindAsync(id);

        if (enrollmentToUpdate == null) return NotFound();

        if (await TryUpdateModelAsync(
                enrollmentToUpdate,
                "course",
                e => e.EnrollmentType, e => e.StartDate, e => e.EndDate, e => e.StudentId))
        {
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        PopulateStudentsDropDownList(_context, enrollmentToUpdate.StudentId);
        return Page();
    }
}