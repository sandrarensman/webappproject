using SchoolApp.Models;

namespace SchoolApp.ViewModels;

public class GroupIndexData
{
    public IEnumerable<Group> Groups { get; init; }
    public IEnumerable<Student> Students { get; set; }
}