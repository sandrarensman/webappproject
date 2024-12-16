using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class EnrollmentService(IConfiguration configuration, DefaultContext context)
    : BaseService<Enrollment>(configuration), IEnrollmentService
{
    private readonly DefaultContext _context = context;

    public async Task<PaginatedListHelper<Enrollment>> GetPaginatedEnrollmentsAsync(
        string sortOrder,
        string searchString,
        int pageIndex,
        int pageSize)
    {
        IQueryable<Enrollment> enrollmentsIq = _context.Enrollments
            .Include(e => e.Student);

        if (!string.IsNullOrEmpty(searchString))
            enrollmentsIq = enrollmentsIq.Where(e =>
                e.Student.FirstName.ToLower().Contains(searchString) ||
                e.Student.LastName.ToLower().Contains(searchString));

        var sortOptions = new Dictionary<string, Func<IQueryable<Enrollment>, IQueryable<Enrollment>>>
        {
            { "student", q => q.OrderBy(e => e.Student.FirstName) },
            { "student_desc", q => q.OrderByDescending(e => e.Student.FirstName) },
            { "date", q => q.OrderBy(e => e.StartDate) },
            { "date_desc", q => q.OrderByDescending(e => e.StartDate) },
            { "type", q => q.OrderBy(e => e.EnrollmentType) },
            { "type_desc", q => q.OrderByDescending(e => e.EnrollmentType) },
            { "default", q => q.OrderBy(e => e.Student.FirstName) }
        };

        return await GetPaginatedAsync(enrollmentsIq, sortOrder, sortOptions, pageIndex, pageSize);
    }
}