
using Sea.Application;
using Sea.Share.Redis;

namespace Sea.Api.Configurations;
public static partial class ApiConfigurations
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {

        // Add services to the container.
        var configuration = builder.Configuration;

        _ = builder.Services.AddFrameworkConfiguration(configuration);

        _ = builder.Services.AddServicesToContainer(configuration);

        //_ = builder.Services.AddJwtAuthentication(configuration);

        _ = builder.Services.AddRedisServices(configuration);

        _ = builder.Services.AddSeriLogAndElaticSearchConfigureServices(configuration);

    }
}
