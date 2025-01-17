using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class StudentSelectionService(DefaultContext context) : IStudentSelectionService
{
    public async Task<SelectList> GetStudentDropdownListAsync(object selectedStudent = null)
    {
        var studentsQuery = from s in context.Students
            orderby s.FirstName
            select s;

        var students = await studentsQuery.AsNoTracking().ToListAsync();

        return new SelectList(students, nameof(Student.StudentId), nameof(Student.FullName), selectedStudent);
    }
}