using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Test.UnitTest.MockData;

namespace SchoolApp.Test.UnitTest.Helpers;

public class PaginatedListHelperDynamicTest : IDisposable
{
    private readonly MockDefaultContext _context;

    public PaginatedListHelperDynamicTest()
    {
        _context = new MockDefaultContext();
    }

    [Theory]
    [InlineData(0, 1, 10)]
    [InlineData(25, 1, 10)]
    [InlineData(25, 3, 10)]
    [InlineData(5, 1, 10)]
    public async Task PaginatedListHelper_CreateAsync_VariousScenarios(int totalItems, int pageIndex, int pageSize)
    {
        // Arrange
        AddDynamicStudents(totalItems);

        // Act
        var result = await PaginatedListHelper<Student>.CreateAsync(_context.Students, pageIndex, pageSize);

        // Assert
        Assert.Equal(Math.Min(pageSize, totalItems - (pageIndex - 1) * pageSize), result.Items.Count);
        Assert.Equal(pageIndex, result.PageIndex);
        Assert.Equal((int)Math.Ceiling((double)totalItems / pageSize), result.TotalPages);
    }

    private void AddDynamicStudents(int count)
    {
        var students = Enumerable.Range(1, count).Select(i => new Student
        {
            StudentId = i,
            FirstName = $"Student{i}",
            LastName = $"LastName{i}",
            EmailAddress = $"student{i}@example.com",
            PhoneNumber = $"06 {i:D8}"
        });

        _context.Students.AddRange(students);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
