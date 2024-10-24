using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments
{
    public class EditModel(SchoolContext context) : StudentNamePageModel
    {
        private readonly SchoolContext _context = context;

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment =  await _context.Enrollments
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentID == id);

            if (enrollment == null)
            {
                return NotFound();
            }
            Enrollment = enrollment;

            // Select current DepartmentID.
            PopulateStudentsDropDownList(_context, Enrollment.StudentID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollmentToUpdate = await _context.Enrollments.FindAsync(id);

            if (enrollmentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                 enrollmentToUpdate,
                 "course",   // Prefix for form value.
                   e => e.EnrollmentType, e => e.StartDate, e => e.EndDate, e => e.StudentID))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select DepartmentID if TryUpdateModelAsync fails.
            PopulateStudentsDropDownList(_context, enrollmentToUpdate.StudentID);
            return Page();
        }
    }
}
