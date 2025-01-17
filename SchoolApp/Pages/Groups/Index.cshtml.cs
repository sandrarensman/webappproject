using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Pages.Groups;

public class IndexModel(DefaultContext context) : PageModel
{
    public GroupMembershipDto GroupData { get; set; }
    
    public int GroupId { get; set; }

    public IList<Group> Group { get; set; } = null!;

    public async Task OnGetAsync()
    {
        GroupData = new GroupMembershipDto
        {
            Groups = await context.Groups
                .Include(g => g.Students.OrderBy(s => s.FirstName))
                .OrderBy(g => g.Day)
                .ToListAsync()
        };
    }
    
    public async Task<IActionResult> OnGetGroupStudentsAsync(int id)
    {
        var group = await context.Groups
            .Include(g => g.Students)
            .SingleOrDefaultAsync(g => g.GroupId == id);

        if (group == null)
        {
            return NotFound();
        }

        var students = group.Students.OrderBy(s => s.FirstName);
        return Partial("_GroupStudentsPartial", new GroupStudentsViewModel
        {
            GroupName = group.Name,
            Students = students
        });
    }
}