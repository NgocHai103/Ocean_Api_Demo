namespace Sea.Share.Redis;

public readonly struct RedisConstant
{
    public const string KeyCommand = "KEYS";

    public const string Sea_Prefix = "sea";
    public static readonly string Sea_Module_Prefix = $"{Sea_Prefix}:module";
    public static readonly string Sea_Request_Prefix = $"{Sea_Module_Prefix}:request";

}