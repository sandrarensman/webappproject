using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments;

public class CreateModel(DefaultContext context) : StudentNamePageModel
{
    [BindProperty] public Enrollment Enrollment { get; set; }

    [BindProperty] public int? StudentId { get; set; }

    public IActionResult OnGet(int? id)
    {
        if (id != null) StudentId = id;
        PopulateStudentsDropDownList(context, id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var emptyEnrollment = new Enrollment();

        if (await TryUpdateModelAsync(
                emptyEnrollment,
                "enrollment",
                e => e.EnrollmentId, e => e.EnrollmentType, e => e.StartDate, e => e.EndDate, e => e.StudentId))
        {
            context.Enrollments.Add(emptyEnrollment);
            await context.SaveChangesAsync();

            if (StudentId.HasValue) return RedirectToPage("/Details", new { id = StudentId });

            return RedirectToPage("./Index");
        }

        PopulateStudentsDropDownList(context, emptyEnrollment.StudentId);
        return Page();
    }
}