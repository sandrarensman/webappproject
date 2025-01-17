using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class DeleteModel(
    DefaultContext context,
    ILogger<CreateModel> logger) : PageModel
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
        var group = await context.Groups.FindAsync(id);
        if (group == null) return NotFound();
        
        Group = group;
        
        try
        {
            context.Groups.Remove(Group);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            logger.LogError("Error deleting group: {Error}", ex.Message);
            ModelState.AddModelError("", "An error occurred while deleting the record.");
            return Page();
        }
    }
}