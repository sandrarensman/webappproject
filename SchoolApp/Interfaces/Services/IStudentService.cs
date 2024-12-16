using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Interfaces.Services;

public interface IStudentService
{
    Task<PaginatedListHelper<Student>> GetPaginatedStudentsAsync(string sortOrder, string searchString, int pageIndex,
        int pageSize);

    string GeneratePageSizeDropdownHtml(int pageSize);
    int GetPageSize(int pageSize);
}