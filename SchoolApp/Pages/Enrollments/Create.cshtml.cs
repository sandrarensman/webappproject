using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments
{
    public class CreateModel(SchoolContext context) : StudentNamePageModel
    {
        private readonly SchoolContext _context = context;

        [BindProperty]
        public Enrollment Enrollment { get; set; }

        [BindProperty]
        public int? StudentID { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                StudentID = id;
            }
            PopulateStudentsDropDownList(_context, id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyEnrollment = new Enrollment();

            if (await TryUpdateModelAsync(
                 emptyEnrollment,
                 "enrollment",   // Prefix for form value.
                 e => e.EnrollmentID, e => e.EnrollmentType, e => e.StartDate, e => e.EndDate, e => e.StudentID))
            {
                _context.Enrollments.Add(emptyEnrollment);
                await _context.SaveChangesAsync();

                if (StudentID.HasValue)
                {
                    return RedirectToPage("/Details", new { id = StudentID });
                }
                else
                {
                    return RedirectToPage("./Index");
                }
            }

            // Select StudentID if TryUpdateModelAsync fails.
            PopulateStudentsDropDownList(_context, emptyEnrollment.StudentID);
            return Page();
        }
    }
}
