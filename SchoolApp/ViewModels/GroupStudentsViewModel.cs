using SchoolApp.Models;

namespace SchoolApp.ViewModels;

public class GroupStudentsViewModel
{
    public string GroupName { get; set; }
    public IEnumerable<Student> Students { get; set; }
}
