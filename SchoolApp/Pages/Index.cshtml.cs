using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students
{
    public class IndexModel(SchoolContext context, IConfiguration configuration) : PageModel
    {
        private readonly SchoolContext _context = context;

        private readonly IConfiguration _configuration = configuration;

        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public int PageSize { get; set; }

        public PaginatedListHelper<Student> Students { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter,
                                string searchString, int pageSize, int? pageIndex)
        {
            CurrentSort = sortOrder;
            LastNameSort = string.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            FirstNameSort = sortOrder == "first_name" ? "first_name_desc" : "first_name";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter?.ToLower();
            }

            CurrentFilter = searchString;

            IQueryable<Student> studentsIQ = from s in _context.Students select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s =>
                    s.LastName.ToLower()
                        .Contains(searchString) ||
                    s.FirstName.ToLower()
                        .Contains(searchString));
            }

            studentsIQ = sortOrder switch
            {
                "last_name_desc"  => studentsIQ.OrderByDescending(s => s.LastName),
                "first_name"      => studentsIQ.OrderBy(s => s.FirstName),
                "first_name_desc" => studentsIQ.OrderByDescending(s => s.FirstName),
                _                 => studentsIQ.OrderBy(s => s.LastName)
            };

            PageSize = pageSize == 0 ? int.Parse(_configuration["PageSize"]) : pageSize;
            Students = await PaginatedListHelper<Student>
                .CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, PageSize);
        }
    }
}
