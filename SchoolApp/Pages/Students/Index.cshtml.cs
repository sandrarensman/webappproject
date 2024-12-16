using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolApp.Helpers;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Pages.Students;

public class IndexModel(IStudentService studentService) : PageModel
{
    private readonly IStudentService _studentService = studentService;

    public string FirstNameSort { get; set; }
    public string LastNameSort { get; set; }

    public string CurrentFilter { get; set; }
    public string CurrentSort { get; set; }

    public int PageSize { get; set; }
    public string PageSizeDropdownHtml { get; private set; } = string.Empty;

    public PaginatedListHelper<Student> Students { get; set; }

    public async Task OnGetAsync(string sortOrder, string currentFilter,
        string searchString, int pageSize, int? pageIndex)
    {
        CurrentSort = sortOrder ?? "default";

        FirstNameSort = CurrentSort == "first_name" ? "first_name_desc" : "first_name";
        LastNameSort = CurrentSort == "last_name" ? "last_name_desc" : "last_name";

        if (searchString != null)
            pageIndex = 1;
        else
            searchString = currentFilter?.ToLower();

        CurrentFilter = searchString;

        PageSize = _studentService.GetPageSize(pageSize);
        Students = await _studentService.GetPaginatedStudentsAsync(sortOrder, searchString, pageIndex ?? 1, PageSize);

        PageSizeDropdownHtml = _studentService.GeneratePageSizeDropdownHtml(PageSize);
    }
}