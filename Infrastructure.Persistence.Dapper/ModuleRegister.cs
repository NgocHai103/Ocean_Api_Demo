using Infrastructure.Persistence.Dapper.Core.Internals;
using Infrastructure.Persistence.Dapper.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Persistence.Dapper;

public static class ModuleRegister
{
    public static IServiceCollection AddDapperPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = InternalEntitiesMapping.InternalEntityConfigurations;
        _ = services.AddDbContexts(configuration);
        return services;
    }
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped(x => new TestDbContext(configuration.GetConnectionString("GIISCommunicationConnection") ?? throw new ArgumentException("ConnectionString Invalid")));

        return services;
    }
}
