using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class DetailsModel(DefaultContext context) : PageModel
{
    public GroupMembershipDto GroupData { get; set; }

    public List<Enrollment> Enrollments { get; set; }

    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var student = await context.Students
            .Include(s => s.Groups)
            .Include(s => s.Enrollments)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null) return NotFound();

        Student = student;
        return Page();
    }
}