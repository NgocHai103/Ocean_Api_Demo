using static Sea.Share.ElasticSearch.ApiElasticConfigurations;

namespace Sea.Api.Configurations;
public static partial class ApiConfigurations
{
    public static IServiceCollection AddSeriLogAndElaticSearchConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddElasticsearch(configuration);
        ConfigureSerilog(configuration);

        return services;
    }
}