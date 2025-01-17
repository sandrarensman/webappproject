using SchoolApp.Models;

namespace SchoolApp.Interfaces.Helpers;

public interface IStudentValidator
{
    Task<List<Student>> ValidateStudentsAsync(List<int> studentIds);
}