using Infrastructure.Persistence.Dapper;
using Infrastructure.Persistence.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sea.Infrastructure.Exceptions;

namespace Sea.Infrastructure;

public static class ModuleRegister
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
       // _ = services.AddDapperPersistenceServices(configuration);

        _ = services.AddEFPersistenceServices(configuration);

        _ = services.Configure<MvcOptions>(o => o.Filters.Add<ErrorHandlingMiddleware>());

        return services;
    }
}