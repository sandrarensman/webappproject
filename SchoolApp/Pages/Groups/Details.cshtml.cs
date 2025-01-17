using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class DetailsModel(DefaultContext context) : PageModel
{
    public GroupMembershipDto GroupData { get; set; }

    public Group Group { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var group = await context.Groups
            .Where(g => g.GroupId == id)
            .Include(g => g.Students.OrderBy(s => s.FirstName))
            .ThenInclude(s => s.Enrollments.OrderBy(e => e.StartDate)) 
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound();
        }

        Group = group;
        return Page();
    }
}