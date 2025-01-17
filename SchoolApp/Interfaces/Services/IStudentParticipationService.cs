using SchoolApp.DTOs;
using SchoolApp.Models;

namespace SchoolApp.Interfaces.Services;

public interface IStudentParticipationService
{
    public List<StudentParticipationDto> GetGroupParticipationData(Student student);

    public void UpdateStudentGroups(string[] selectedGroups, Student studentToUpdate);
}