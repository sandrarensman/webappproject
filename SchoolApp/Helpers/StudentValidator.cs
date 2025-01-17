using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Interfaces.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Helpers;

public class StudentValidator(DefaultContext context) : IStudentValidator
{
    public async Task<List<Student>> ValidateStudentsAsync(List<int> studentIds)
    {
        if (studentIds == null || studentIds.Count == 0)
        {
            return [];
        }
        
        var validStudents = await context.Students
            .Where(s => studentIds.Contains(s.StudentId))
            .ToListAsync();

        if (validStudents.Count != studentIds.Count)
        {
            throw new InvalidOperationException("One or more student IDs are invalid.");
        }

        return validStudents;
    }
}
