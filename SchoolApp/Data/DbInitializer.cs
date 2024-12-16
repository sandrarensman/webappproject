using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Data;

public class DbInitializer(JsonDataLoader jsonDataLoader)
{
    public void Initialize(DefaultContext context)
    {
        if (context.Students.Any()) return;
        
        var students = jsonDataLoader.LoadFromJson<Student>("./Data/SeedData/StudentData.json");
        context.Students.AddRange(students);
        
        var enrollments = jsonDataLoader.LoadFromJson<Enrollment>("./Data/SeedData/EnrollmentData.json");
        context.Enrollments.AddRange(enrollments);
        
        var groups = jsonDataLoader.LoadFromJson<Group>("./Data/SeedData/GroupData.json");
        foreach (var groupFromDb in groups.Select(group => new Group(group.Day, group.StartTime, group.EndTime, group.Level, group.StudentIds)
                 {
                     Students = students.Where(s => group.StudentIds.Contains(s.StudentId)).ToList()
                 }))
        {
            context.Groups.Add(groupFromDb);
        }
        
        context.SaveChanges();
    }
}
