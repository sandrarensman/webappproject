using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.DTOs;
using SchoolApp.Interfaces.Services;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class StudentParticipationService(DefaultContext context) : IStudentParticipationService
{
    public List<StudentParticipationDto> GetGroupParticipationData(Student student)
    {
        var allGroups = context.Groups;
        var studentGroups = new HashSet<int>(student.Groups.Select(g => g.GroupId));

        return allGroups.Select(group => new StudentParticipationDto
        {
            GroupId = group.GroupId,
            Name = group.Name,
            Participation = studentGroups.Contains(group.GroupId)
        }).ToList();
    }

    public void UpdateStudentGroups(string[] selectedGroups, Student studentToUpdate)
    {
        if (selectedGroups == null)
        {
            studentToUpdate.Groups.Clear();
            return;
        }
        
        studentToUpdate.Groups ??= new List<Group>();
        
        var selectedGroupIds = new HashSet<int>(selectedGroups.Select(int.Parse));
        var studentGroupIds = new HashSet<int>(studentToUpdate.Groups.Select(g => g.GroupId));

        var groupsToAdd = selectedGroupIds.Except(studentGroupIds);
        foreach (var groupId in groupsToAdd)
        {
            var group = context.Groups.Find(groupId);
            if (group != null)
            {
                studentToUpdate.Groups.Add(group);
            }
        }
        
        var groupsToRemove = studentGroupIds.Except(selectedGroupIds);
        foreach (var groupId in groupsToRemove)
        {
            var groupToRemove = studentToUpdate.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (groupToRemove != null)
            {
                studentToUpdate.Groups.Remove(groupToRemove);
            }
        }
    }
}