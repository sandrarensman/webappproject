using Microsoft.Extensions.Configuration;
using Moq;
using SchoolApp.Services;
using SchoolApp.Test.UnitTest.MockData;
using SchoolApp.Test.UnitTest.TestHelpers;

namespace SchoolApp.Test.UnitTest.Services;

public class StudentServiceStaticMockDataTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly StudentService _studentService;

    public StudentServiceStaticMockDataTests(TestFixture fixture)
    {
        _fixture = fixture;
        var mockConfiguration = new Mock<IConfiguration>();

        _studentService = new StudentService(mockConfiguration.Object, fixture.Context);
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_ReturnsPaginatedResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("", "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Items.Count);
        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_Search_ReturnsFilteredStudents()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;
        const string searchString = "Verbey";

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("", searchString, pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.All(result.Items,
            student =>
            {
                Assert.Contains(searchString.ToLower(), student.FirstName.ToLower() + " " + student.LastName.ToLower());
            });
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_NoSearch_ReturnsAllStudents()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("", "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Items.Count);
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_Sorting_ReturnsSortedResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("first_name_desc", "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Items.SequenceEqual(result.Items.OrderByDescending(s => s.FirstName)));
    }

    [Fact]
    public void GetPageSize_ReturnsConfiguredPageSize_WhenPageSizeIsZero()
    {
        // Arrange
        const int pageSize = 0;
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["PageSize"]).Returns("10");

        var studentService = new StudentService(mockConfiguration.Object, new MockDefaultContext());

        // Act
        var result = studentService.GetPageSize(pageSize);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void GeneratePageSizeDropdownHtml_ReturnsCorrectHtml()
    {
        // Arrange
        const int pageSize = 10;

        // Act
        var html = _studentService.GeneratePageSizeDropdownHtml(pageSize);

        // Assert
        Assert.Contains("<form method='get' style='display: inline; margin: 0; padding: 0; border: none;'>", html);
        Assert.Contains("<option value=\'10\' selected>", html);
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_WithDifferentPageSize_ReturnsCorrectPageSize()
    {
        // Arrange
        const int pageSize = 2;
        const int pageIndex = 1;

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("", "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Items.Count);
    }

    [Fact]
    public async Task GetPaginatedStudentsAsync_ShouldReturnEmptyResult_WhenNoMatchesForSearch()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;
        const string searchString = "NonExistentName";

        // Act
        var result = await _studentService.GetPaginatedStudentsAsync("", searchString, pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    [Fact]
    public void GetPageSize_ReturnsProvidedPageSize_WhenPageSizeIsNonZero()
    {
        // Arrange
        const int pageSize = 25;

        // Act
        var result = _studentService.GetPageSize(pageSize);

        // Assert
        Assert.Equal(pageSize, result);
    }

    [Fact]
    public void GetPageSize_ShouldThrowException_WhenConfigurationIsMissing()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["PageSize"]).Returns((string?)null);

        var studentService = new StudentService(mockConfiguration.Object, new MockDefaultContext());

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => studentService.GetPageSize(0));
        Assert.Equal("The input string '' was not in a correct format.", exception.Message);
    }
}