namespace Sea.Share.Redis.Services;

public interface IRedisService<TRedisDto> where TRedisDto : class
{
    public ValueTask<TRedisDto> Get(string group, string key);

    public ValueTask<IDictionary<string, TRedisDto>> GetBulk(string group, params string[] keys);

    public ValueTask<IDictionary<string, TRedisDto>> GetAll(string group);

    public ValueTask<bool> Set(string group, string key, TRedisDto value);

    public ValueTask<bool> SetBulk(string group, IDictionary<string, TRedisDto> fields);

    public ValueTask<bool> Delete(string group, string key);

    public ValueTask<bool> DeleteBulk(string group, params string[] keys);

    public ValueTask<bool> DeleteAll(string group);

    public ValueTask<IDictionary<string, IDictionary<string, TRedisDto>>> GetGroup(string groupPreffix);

    public ValueTask<bool> DeleteGroup(string groupPreffix);
}
