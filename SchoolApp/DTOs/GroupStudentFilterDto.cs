using SchoolApp.Models;

namespace SchoolApp.DTOs;

public class GroupStudentFilterResult
{
    public List<Student> AddedStudents { get; set; } = [];
    public List<Student> CurrentStudents { get; set; } = [];
    public List<Student> AvailableStudents { get; set; } = [];
}