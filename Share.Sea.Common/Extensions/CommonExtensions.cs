using StackExchange.Redis;

namespace Share.Sea.Common.Extensions;
public static class CommonExtensions
{
    public static bool IsNotEmptyAndNull(this RedisKey[] value) => value is not null && value.All(v => v.ToString() is not null and not "");

    public static bool IsWhiteSpaceOrNull(this string value) => value is not null or "";

    public static bool IsNotWhiteSpaceAndNull(this string value) => value is not null and "";

    public static bool IsNull(this object value) => value is null;

    public static bool AllWhiteSpaceOrNull(this string[] value) => value is null || value.Any(s => s is null or "");

    // public static bool IsEmptyOrNull(this IDictionary<string, object> value) => value.IsEmptyOrNull();
}
