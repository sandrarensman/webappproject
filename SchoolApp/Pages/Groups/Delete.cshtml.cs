using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class DeleteModel(DefaultContext context) : PageModel
{
    [BindProperty] public Group Group { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || context.Groups == null) return NotFound();

        var group = await context.Groups.FirstOrDefaultAsync(m => m.GroupId == id);

        if (group == null) return NotFound();

        Group = group;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || context.Groups == null) return NotFound();
        var group = await context.Groups.FindAsync(id);

        if (group != null)
        {
            Group = group;
            context.Groups.Remove(Group);
            await context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}