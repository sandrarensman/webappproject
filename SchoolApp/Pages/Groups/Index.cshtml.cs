using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Pages.Groups;

public class IndexModel(DefaultContext context) : PageModel
{
    private readonly DefaultContext _context = context;

    public GroupIndexData GroupData { get; set; }
    public int GroupId { get; set; }
    public int StudentId { get; set; }

    public IList<Group> Group { get; set; } = default!;

    public async Task OnGetAsync(int? id)
    {
        GroupData = new GroupIndexData
        {
            Groups = await _context.Groups
                .Include(g => g.Students)
                .OrderBy(g => g.Day)
                .ToListAsync()
        };

        if (id != null)
        {
            GroupId = id.Value;
            var group = GroupData.Groups.Single(g => g.GroupId == GroupId);
            GroupData.Students = group.Students.OrderBy(s => s.LastName);
        }
    }
}