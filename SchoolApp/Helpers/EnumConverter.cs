using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolApp.Helpers;

public class EnumConverter<T> : JsonConverter<T> where T : struct
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var underlyingType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;

        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeof(T).Name}");
        var enumString = reader.GetString();
        if (Enum.TryParse(underlyingType, enumString, true, out var value))
        {
            return (T)value;
        }

        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}