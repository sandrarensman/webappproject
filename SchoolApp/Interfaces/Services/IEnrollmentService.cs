using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Interfaces.Services;

public interface IEnrollmentService
{
    Task<PaginatedListHelper<Enrollment>> GetPaginatedEnrollmentsAsync(string sortOrder, string searchString,
        int pageIndex, int pageSize);

    string GeneratePageSizeDropdownHtml(int pageSize);
    int GetPageSize(int pageSize);
}