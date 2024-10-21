using System.Text.Json;

namespace Share.Sea.Common.Helpers;

public static class JsonHelper
{
    public static T? Deserialize<T>(this string jsonString)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var options = jsonSerializerOptions;
        return JsonSerializer.Deserialize<T>(jsonString, options);
    }

    public static string Serialize<T>(this T obj)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var options = jsonSerializerOptions;
        return JsonSerializer.Serialize(obj, options);
    }
}