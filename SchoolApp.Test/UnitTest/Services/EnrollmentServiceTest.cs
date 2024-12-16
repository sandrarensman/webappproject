using Microsoft.Extensions.Configuration;
using Moq;
using SchoolApp.Services;
using SchoolApp.Test.UnitTest.TestHelpers;

namespace SchoolApp.Test.UnitTest.Services;

public class EnrollmentServiceTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["PageSize"]).Returns("10");

        _enrollmentService = new EnrollmentService(mockConfiguration.Object, fixture.Context);
    }

    [Fact]
    public async Task GetPaginatedEnrollmentsAsync_ReturnsPaginatedResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;

        // Act
        var result = await _enrollmentService.GetPaginatedEnrollmentsAsync("", "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Items.Count);
        Assert.True(result.TotalPages > 0);
    }

    [Fact]
    public async Task GetPaginatedEnrollmentsAsync_SearchFilter_ReturnsFilteredResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;
        const string searchString = "Jan";

        // Act
        var result = await _enrollmentService.GetPaginatedEnrollmentsAsync("", searchString, pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.All(result.Items,
            enrollment =>
            {
                Assert.Contains(searchString.ToLower(),
                    enrollment.Student.FirstName.ToLower() + " " + enrollment.Student.LastName.ToLower());
            });
    }

    [Fact]
    public async Task GetPaginatedEnrollmentsAsync_Sorting_ReturnsSortedResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;
        const string sortOrder = "student_desc";

        // Act
        var result = await _enrollmentService.GetPaginatedEnrollmentsAsync(sortOrder, "", pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Items.SequenceEqual(result.Items.OrderByDescending(e => e.Student.FirstName)
            .ThenByDescending(e => e.Student.LastName)));
    }
}