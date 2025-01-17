using System.Text.Json;

namespace SchoolApp.Helpers;

public class JsonDataLoader(JsonSerializerOptions jsonOptions)
{
    public List<T> LoadFromJson<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file {filePath} was not found.");
        }

        var jsonData = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(jsonData, jsonOptions) ?? [];
    }
}