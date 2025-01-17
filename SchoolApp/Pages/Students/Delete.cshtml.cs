using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class DeleteModel(DefaultContext context, ILogger<DeleteModel> logger) : PageModel
{
    [BindProperty] public Student Student { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
    {
        if (id == null || context.Students == null) return NotFound();

        var student = await context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (student == null) return NotFound();

        Student = student;

        if (saveChangesError.GetValueOrDefault()) ErrorMessage = $"Delete {id} failed. Try again";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        var student = await context.Students.FindAsync(id);
        if (student == null) return NotFound();
        
        Student = student;
        
        try
        {
            context.Students.Remove(student);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            logger.LogError("Error deleting student: {Error}", ex.Message);
            ModelState.AddModelError("", "An error occurred while deleting the record.");
            return Page();
        }
    }
}