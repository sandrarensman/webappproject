using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Test.UnitTest.MockData;

public sealed class MockDefaultContext : DefaultContext
{
    public MockDefaultContext(
        IQueryable<Student>? mockStudents = null,
        IQueryable<Enrollment>? mockEnrollments = null,
        IQueryable<Group>? mockGroups = null)
        : base(new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options)
    {
        if (mockStudents != null) Students.AddRange(mockStudents);

        if (mockEnrollments != null) Enrollments.AddRange(mockEnrollments);

        if (mockGroups != null) Groups.AddRange(mockGroups);

        SaveChanges();
    }
}