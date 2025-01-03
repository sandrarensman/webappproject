using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Pages.Students;

public class DetailsModel(DefaultContext context) : PageModel
{
    private readonly DefaultContext _context = context;

    public GroupIndexData GroupData { get; set; }

    public List<Enrollment> Enrollments { get; set; }

    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .Include(s => s.Groups)
            .Include(s => s.Enrollments)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null) return NotFound();

        Student = student;
        return Page();
    }
}