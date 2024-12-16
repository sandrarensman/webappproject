using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class DeleteModel(
    DefaultContext context,
    ILogger<DeleteModel> logger) : PageModel
{
    private readonly DefaultContext _context = context;
    private readonly ILogger<DeleteModel> _logger = logger;

    [BindProperty] public Student Student { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (student == null) return NotFound();

        Student = student;

        if (saveChangesError.GetValueOrDefault()) ErrorMessage = $"Delete {id} failed. Try again";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .Include(s => s.Groups)
            .SingleAsync(s => s.StudentId == id);

        if (student == null) return RedirectToPage("./Index");

        var enrollments = await _context.Enrollments
            .Where(e => e.StudentId == id)
            .ToListAsync();

        try
        {
            foreach (var enrollment in enrollments) _context.Enrollments.Remove(enrollment);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, ErrorMessage);

            return RedirectToAction("./Delete",
                new { id, saveChangesError = true });
        }
    }
}