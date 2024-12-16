using SchoolApp.Models;

namespace SchoolApp.Test.UnitTest.TestHelpers;

public static class StudentMappingHelper
{
    public static Dictionary<string, Student> GetStudentMap(List<Student> students)
    {
        return students.ToDictionary(
            s => $"{s.FirstName} {(string.IsNullOrEmpty(s.Prefix) ? "" : s.Prefix + " ")}{s.LastName}".Trim(),
            s => s
        );
    }
}