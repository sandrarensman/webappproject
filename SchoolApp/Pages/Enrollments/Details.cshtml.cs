﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments
{
    public class DetailsModel(SchoolContext context) : PageModel
    {
        private readonly SchoolContext _context = context;

        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentID == id);

            if (enrollment == null)
            {
                return NotFound();
            }
            else
            {
                Enrollment = enrollment;
            }
            return Page();
        }
    }
}
