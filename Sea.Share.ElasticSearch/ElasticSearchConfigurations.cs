using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Sea.Share.ElasticSearch.Services;
using Sea.Share.ElasticSearch.Services.Implements;
using Serilog;
using static Elasticsearch.Net.CertificateValidations;
using static Sea.Share.ElasticSearch.ElasticConstants;
using DataStreamName = Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName;

namespace Sea.Share.ElasticSearch;
public static class ApiElasticConfigurations
{
    public static void ConfigureSerilog(IConfiguration configuration) => Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.Elasticsearch([new Uri(configuration[UrlTag] ?? "")], opts =>
                        {
                            opts.DataStream = new DataStreamName(configuration[TopicLog] ?? "", "api", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production");
                            opts.BootstrapMethod = BootstrapMethod.Failure;
                            opts.ConfigureChannel = channelOpts => channelOpts.BufferOptions = new BufferOptions
                            {
                                ExportMaxConcurrency = 10
                            };
                        }, transport =>
                        {
                            _ = transport.DisableDirectStreaming();
                            _ = transport.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
                            _ = transport.Authentication(new BasicAuthentication(configuration[UsernameTag] ?? "", configuration[PwdTag] ?? ""));
                        })
                        .CreateLogger();

    public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var urls = configuration.GetSection(UrlTag).GetChildren();

        var connectionPool = urls.Any()
            ? (IConnectionPool)new StaticConnectionPool(urls.Select(s => new Uri(s.Value)).ToArray())
            : new SingleNodeConnectionPool(new Uri(configuration.GetSection(UrlTag).Value));

        var settings = new ConnectionSettings(connectionPool)
            .DefaultIndex(configuration.GetSection(DefaultIndex)?.Value)
            .EnableDebugMode()
            .PrettyJson()
            .RequestTimeout(TimeSpan.FromMinutes(2));

        if (configuration.GetSection(UsernameTag).Value is not null)
        {
            _ = settings.ServerCertificateValidationCallback((o, certificate, chain, errors) => true);
            _ = settings.ServerCertificateValidationCallback(AllowAll);
            _ = settings.BasicAuthentication(configuration.GetSection(UsernameTag).Value, configuration.GetSection(PwdTag).Value);
        }

        var client = new ElasticClient(settings);
        _ = services.AddSingleton<IElasticClient>(client);
        _ = services.AddScoped(typeof(IElasticSearchService<,>), typeof(ElasticSearchService<,>));

        return services;
    }

    //public static DeleteIndexResponse DeleteSampleIndex(this IElasticClient client) => _indexUser is not null ? client.Indices.Delete(_indexUser) : default;
}
