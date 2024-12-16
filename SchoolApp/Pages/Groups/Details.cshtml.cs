using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Pages.Groups;

public class DetailsModel(DefaultContext context) : PageModel
{
    private readonly DefaultContext _context = context;

    public GroupIndexData GroupData { get; set; }

    public Group Group { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Groups == null) return NotFound();

        var group = await _context.Groups
            .Include(g => g.Students
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName))
            .ThenInclude(s => s.Enrollments
                .OrderBy(e => e.StartDate))
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.GroupId == id);
        if (group == null) return NotFound();

        Group = group;
        return Page();
    }
}