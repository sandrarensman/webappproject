using System.Text.Json;
using SchoolApp.Helpers;
using SchoolApp.Models;

namespace SchoolApp.Test.UnitTest.Helpers;

public class JsonDataLoaderTest
{
    [Fact]
    public void LoadFromJson_ShouldLoadDataCorrectly()
    {
        // Arrange
        const string testFilePath = "TestData/sample_students.json";
        File.WriteAllText(testFilePath, "[{\"StudentId\":1,\"FirstName\":\"Test\",\"LastName\":\"Student\"}]");
        
        var jsonOptions = new JsonSerializerOptions();
        var jsonDataLoader = new JsonDataLoader(jsonOptions);

        // Act
        var result = jsonDataLoader.LoadFromJson<Student>(testFilePath);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result.First().StudentId);
        Assert.Equal("Test", result.First().FirstName);

        // Cleanup
        File.Delete(testFilePath);
    }
}