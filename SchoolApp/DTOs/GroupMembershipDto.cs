using SchoolApp.Models;

namespace SchoolApp.DTOs;

public class GroupMembershipDto
{
    public IEnumerable<Group> Groups { get; init; }
    public IEnumerable<Student> Students { get; set; }
}