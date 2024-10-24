using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Enrollments
{
    public class IndexModel(SchoolContext context, IConfiguration configuration) : PageModel
    {
        private readonly SchoolContext _context = context;

        private readonly IConfiguration _configuration = configuration;

        public string StudentSort { get; set; }
        public string DateSort { get; set; }
        public string TypeSort { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public int PageSize { get; set; }

        public PaginatedListHelper<Enrollment> Enrollments { get;set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter,
                                string searchString, int pageSize, int? pageIndex)
        {
            CurrentSort = sortOrder;
            StudentSort = string.IsNullOrEmpty(sortOrder) ? "student_desc" : "";
            DateSort = sortOrder == "date" ? "date_desc" : "date";
            TypeSort = sortOrder == "type" ? "type_desc" : "type";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter?.ToLower();
            }

            CurrentFilter = searchString;

            IQueryable<Enrollment> enrollmentsIQ = from e in _context.Enrollments select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                enrollmentsIQ = enrollmentsIQ.Where(e =>
                    e.Student.FirstName.ToLower().Contains(searchString) ||
                    e.Student.LastName.ToLower().Contains(searchString));
            }

            enrollmentsIQ = sortOrder switch
            {
                "student_desc" => enrollmentsIQ
                                        .OrderByDescending(e => e.Student.FirstName)
                                            .ThenBy(e => e.StartDate),
                "date"         => enrollmentsIQ
                                        .OrderBy(e => e.StartDate),
                "date_desc"    => enrollmentsIQ
                                        .OrderByDescending(e => e.StartDate),
                "type"         => enrollmentsIQ
                                        .OrderBy(e => e.EnrollmentType)
                                            .ThenBy(e => e.Student.FirstName),
                "type_desc"    => enrollmentsIQ
                                        .OrderByDescending(e => e.EnrollmentType)
                                            .ThenBy(e => e.Student.FirstName),
                _              => enrollmentsIQ
                                        .OrderBy(e => e.Student.FirstName)
                                            .ThenBy(e => e.StartDate)
            };

            PageSize = pageSize == 0 ? int.Parse(_configuration["PageSize"]) : pageSize;
            Enrollments = await PaginatedListHelper<Enrollment>
                .CreateAsync(enrollmentsIQ
                    .Include(e => e.Student)
                    .AsNoTracking(), pageIndex ?? 1, PageSize);
        }
    }
}
