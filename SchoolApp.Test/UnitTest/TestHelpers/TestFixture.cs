using System.Text.Json;
using SchoolApp.Helpers;
using SchoolApp.Models;
using SchoolApp.Test.UnitTest.MockData;

namespace SchoolApp.Test.UnitTest.TestHelpers;

public class TestFixture : IDisposable
{
    public MockDefaultContext Context { get; private set; }
    private readonly JsonDataLoader _jsonDataLoader;

    public TestFixture()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new EnumConverter<Level>(), new EnumConverter<EnrollmentType>() }
        };

        _jsonDataLoader = new JsonDataLoader(options);

        // Laad de mock data
        var students = LoadMockData<Student>("StudentMockData.json");
        var enrollments = LoadMockData<Enrollment>("EnrollmentMockData.json");
        var groups = LoadMockData<Group>("GroupMockData.json");

        // Maak een nieuwe mock database context
        Context = new MockDefaultContext(mockStudents: students, mockEnrollments: enrollments, mockGroups: groups);
    }

    private IQueryable<T> LoadMockData<T>(string fileName)
    {
        var jsonFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UnitTest", "MockData", "Json");
        var filePath = Path.Combine(jsonFolderPath, fileName);
        return _jsonDataLoader.LoadFromJson<T>(filePath).AsQueryable();
    }

    // Dispose om context op te ruimen
    public void Dispose()
    {
        Context.Dispose();
    }
}