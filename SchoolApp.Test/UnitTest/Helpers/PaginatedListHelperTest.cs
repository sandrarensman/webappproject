using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Test.UnitTest.TestHelpers;

namespace SchoolApp.Test.UnitTest.Helpers;

public class PaginatedListHelperTest(TestFixture fixture) : IClassFixture<TestFixture>
{
    [Fact]
    public async Task PaginatedListHelper_CreateAsync_ReturnsPaginatedResult()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 1;

        // Act
        var result = await PaginatedListHelper<Student>.CreateAsync(fixture.Context.Students, pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.Items.Count);
        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
    }

    // [Theory]
    // [InlineData(0, 1, 10)]
    // [InlineData(25, 1, 10)]
    // [InlineData(25, 3, 10)]
    // [InlineData(5, 1, 10)]
    // public async Task PaginatedListHelper_CreateAsync_VariousScenarios(int totalItems, int pageIndex, int pageSize)
    // {
    //     // Act
    //     var result = await PaginatedListHelper<Student>.CreateAsync(fixture.Context.Students, pageIndex, pageSize);
    //
    //     // Assert
    //     Assert.Equal(Math.Min(pageSize, totalItems - (pageIndex - 1) * pageSize), result.Items.Count);
    //     Assert.Equal(pageIndex, result.PageIndex);
    //     Assert.Equal((int)Math.Ceiling((double)totalItems / pageSize), result.TotalPages);
    // }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(-1, 5)]
    public async Task PaginatedListHelper_CreateAsync_ShouldThrowArgumentException_WhenPageSizeIsZeroOrNegative(int pageIndex, int pageSize)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            PaginatedListHelper<Student>.CreateAsync(fixture.Context.Students, pageIndex, pageSize));
    }
}