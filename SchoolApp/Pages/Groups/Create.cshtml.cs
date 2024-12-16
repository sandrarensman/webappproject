using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Groups;

public class CreateModel(DefaultContext context) : PageModel
{
    [BindProperty] public Group Group { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        context.Groups.Add(Group);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}