using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class CreateModel(DefaultContext context, ILogger<StudentGroupsPageModel> logger) : StudentGroupsPageModel
{
    [BindProperty] public Student Student { get; set; }

    public IActionResult OnGet()
    {
        var student = new Student
        {
            Groups = []
        };

        PopulateGroupParticipationData(context, student);
        return Page();
    }


    public async Task<IActionResult> OnPostAsync(string[] selectedGroups)
    {
        var emptyStudent = new Student();

        if (selectedGroups.Length > 0)
        {
            emptyStudent.Groups = [];
            await context.Groups.LoadAsync();
        }

        foreach (var group in selectedGroups)
        {
            var foundGroup = await context.Groups.FindAsync(int.Parse(group));
            if (foundGroup != null)
                emptyStudent.Groups.Add(foundGroup);
            else
                logger.LogWarning("Group {group} not found", group);
        }

        try
        {
            if (await TryUpdateModelAsync(
                    emptyStudent,
                    "student",
                    s => s.FirstName, s => s.Prefix, s => s.LastName,
                    s => s.PhoneNumber, s => s.EmailAddress, s => s.Motivation))
            {
                context.Students.Add(emptyStudent);
                await context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        PopulateGroupParticipationData(context, emptyStudent);
        return Page();
    }
}