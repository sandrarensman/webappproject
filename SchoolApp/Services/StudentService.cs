using SchoolApp.Data;
using SchoolApp.Helpers;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class StudentService(IConfiguration configuration, DefaultContext context)
    : BaseService<Student>(configuration), IStudentService
{
    private readonly DefaultContext _context = context;

    public async Task<PaginatedListHelper<Student>> GetPaginatedStudentsAsync(
        string sortOrder,
        string searchString,
        int pageIndex,
        int pageSize)
    {
        IQueryable<Student> studentsIq = _context.Students;

        if (!string.IsNullOrEmpty(searchString))
            studentsIq = studentsIq.Where(s =>
                s.LastName.ToLower().Contains(searchString) ||
                s.FirstName.ToLower().Contains(searchString));

        var sortOptions = new Dictionary<string, Func<IQueryable<Student>, IQueryable<Student>>>
        {
            { "last_name", q => q.OrderBy(s => s.LastName) },
            { "last_name_desc", q => q.OrderByDescending(s => s.LastName) },
            { "first_name", q => q.OrderBy(s => s.FirstName) },
            { "first_name_desc", q => q.OrderByDescending(s => s.FirstName) },
            { "default", q => q.OrderBy(s => s.FirstName) }
        };

        return await GetPaginatedAsync(studentsIq, sortOrder, sortOptions, pageIndex, pageSize);
    }
}