using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace SchoolApp.Pages.Students
{
    public class StudentGroupsPageModel : PageModel
    {
        public List<GroupParticipationData> GroupParticipationDataList;

        public void PopulateGroupParticipationData(SchoolContext context,
                                               Student student)
        {
            var allGroups = context.Groups;
            var studentGroups = new HashSet<int>(
                student.Groups.Select(g => g.GroupID));
            GroupParticipationDataList = new List<GroupParticipationData>();
            foreach (var group in allGroups)
            {
                GroupParticipationDataList.Add(new GroupParticipationData
                {
                    GroupID = group.GroupID,
                    Name = group.Name,
                    Participation = studentGroups.Contains(group.GroupID)
                });
            }
        }
    }
}
