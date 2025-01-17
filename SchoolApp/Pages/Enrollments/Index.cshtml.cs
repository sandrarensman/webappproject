using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Pages.Enrollments;

public class IndexModel(EnrollmentService enrollmentService) : PageModel
{
    public string StudentSort { get; set; }
    public string DateSort { get; set; }
    public string TypeSort { get; set; }

    public string CurrentFilter { get; set; }
    public string CurrentSort { get; set; }

    public int PageSize { get; set; }
    public string PageSizeDropdownHtml { get; private set; } = string.Empty;

    public PaginatedListHelper<Enrollment> Enrollments { get; set; }

    public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter,
        string searchString, int pageSize, int? pageIndex)
    {
        CurrentSort = sortOrder ?? "default";

        StudentSort = CurrentSort == "student" ? "student_desc" : "student";
        DateSort = CurrentSort == "date" ? "date_desc" : "date";
        TypeSort = CurrentSort == "type" ? "type_desc" : "type";

        if (searchString != null)
            pageIndex = 1;
        else
            searchString = currentFilter?.ToLower();

        CurrentFilter = searchString;

        PageSize = enrollmentService.GetPageSize(pageSize);
        Enrollments =
            await enrollmentService.GetPaginatedEnrollmentsAsync(sortOrder, searchString, pageIndex ?? 1, PageSize);

        PageSizeDropdownHtml = enrollmentService.GeneratePageSizeDropdownHtml(PageSize);
        
        return Page();
    }
}