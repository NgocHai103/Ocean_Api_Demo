using StackExchange.Redis;

namespace Sea.Share.Redis.ConnectionFactories;

public interface IRedisConnectionFactory
{
    public ConnectionMultiplexer Connection();

    public string ConnectionString();
}
