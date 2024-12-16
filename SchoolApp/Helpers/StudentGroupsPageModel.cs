using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Helpers;

public class StudentGroupsPageModel : PageModel
{
    public List<GroupParticipationData> GroupParticipationDataList;

    public void PopulateGroupParticipationData(DefaultContext context,
        Student student)
    {
        var allGroups = context.Groups;
        var studentGroups = new HashSet<int>(
            student.Groups.Select(g => g.GroupId));
        GroupParticipationDataList = [];
        foreach (var group in allGroups)
            GroupParticipationDataList.Add(new GroupParticipationData
            {
                GroupId = group.GroupId,
                Name = group.Name,
                Participation = studentGroups.Contains(group.GroupId)
            });
    }
}