using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class GroupStudentFilterService(DefaultContext context) : IGroupStudentFilterService
{
    public async Task<GroupStudentFilterResult> FilterStudentsForGroupAsync(
        List<int> addedStudentIds, List<int> currentStudentIds, bool updateCurrentStudents)
    {
        var addedStudents = new List<Student>();
        var currentStudents = new List<Student>();
        
        if (addedStudentIds.Count != 0)
        {
            addedStudents = await context.Students
                .Where(s => addedStudentIds.Contains(s.StudentId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();
        }

        if (updateCurrentStudents)
        {
            currentStudents = await context.Students
                .Where(s => currentStudentIds.Contains(s.StudentId))
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();
        }

        var availableStudents = await context.Students
            .Where(s => !currentStudentIds.Contains(s.StudentId))
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .ToListAsync();

        return new GroupStudentFilterResult
        {
            AddedStudents = addedStudents,
            CurrentStudents = currentStudents,
            AvailableStudents = availableStudents,
        };
    }
}