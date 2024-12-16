using System.Text;
using SchoolApp.Models;

namespace SchoolApp.Helpers;

public static class StudentDropdownHelper
{
    public static string GenerateStudentDropdownHtml(List<Student> availableStudents, List<int> selectedStudentIds)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<select id='SelectedStudentIds' name='SelectedStudentIds' class='form-control' multiple='multiple'>");
        foreach (var student in availableStudents)
        {
            var isSelected = selectedStudentIds.Contains(student.StudentId) ? "selected" : "";
            sb.AppendLine($"<option value='{student.StudentId}' {isSelected}>{student.FullName}</option>");
        }
        sb.AppendLine("</select>");
        return sb.ToString();
    }

    public static string GenerateSelectedStudentsHtml(List<Student> availableStudents, List<int> selectedStudentIds)
    {
        var sb = new StringBuilder();
        foreach (var studentId in selectedStudentIds)
        {
            var student = availableStudents.FirstOrDefault(s => s.StudentId == studentId);
            if (student != null)
            {
                sb.AppendLine($"<li data-student-id='{student.StudentId}'>");
                sb.AppendLine($"{student.FullName} <button type='button' class='btn btn-sm btn-danger' onclick='removeStudent({student.StudentId})'>Remove</button>");
                sb.AppendLine("</li>");
            }
        }
        return sb.ToString();
    }
}