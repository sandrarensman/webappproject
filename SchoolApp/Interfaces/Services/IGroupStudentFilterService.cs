using SchoolApp.DTOs;

namespace SchoolApp.Interfaces.Services;

public interface IGroupStudentFilterService
{
    public Task<GroupStudentFilterResult> FilterStudentsForGroupAsync(
        List<int> addedStudentIds, List<int> currentStudentIds, bool updateCurrentStudents);
}