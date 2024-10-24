using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolApp.Data;
using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolApp.Pages.Students
{
    public class CreateModel(SchoolContext context, ILogger<StudentGroupsPageModel> logger) : StudentGroupsPageModel
    {
        private readonly SchoolContext _context = context;
        private readonly ILogger<StudentGroupsPageModel> _logger = logger;

        public IActionResult OnGet()
        {
            var student = new Student
            {
                Groups = []
            };

            // Provides an empty collection for the foreach loop
            // foreach (var group in Model.GroupParticipationDataList)
            // in the Create Razor page.
            PopulateGroupParticipationData(_context, student);
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }
        

        public async Task<IActionResult> OnPostAsync(string[] selectedGroups)
        {
            var emptyStudent = new Student();

            if (selectedGroups.Length > 0)
            {
                emptyStudent.Groups = [];
                // Load collection with one DB call.
                _context.Groups.Load();
            }

            foreach (var group in selectedGroups)
            {
                var foundGroup = await _context.Groups.FindAsync(int.Parse(group));
                if (foundGroup != null)
                {
                    emptyStudent.Groups.Add(foundGroup);
                }
                else
                {
                    _logger.LogWarning("Group {group} not found", group);
                }
            }

            try
            {
                if (await TryUpdateModelAsync(
                        emptyStudent,
                        "student",   // Prefix for form value.
                        s => s.FirstName, s => s.Prefix, s => s.LastName,
                        s => s.PhoneNumber, s => s.EmailAddress, s => s.Motivation))
                {
                    _context.Students.Add(emptyStudent);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            PopulateGroupParticipationData(_context, emptyStudent);
            return Page();
        }
    }
}
