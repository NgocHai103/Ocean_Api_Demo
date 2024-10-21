using Infrastructure.Persistence.EF.DbContexts;
using Infrastructure.Persistence.EF.DbContexts.Core;
using Infrastructure.Persistence.EF.Repositories;
using Infrastructure.Persistence.EF.UnitOfWorks;
using Infrastructure.Persistence.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sea.Domain.Repositories;
using Sea.Domain.UnitOfWorks;

namespace Infrastructure.Persistence.EFCore;

public static class ModuleRegister
{
    public static IServiceCollection AddEFPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContexts(configuration);
        _ = services.AddServicesToContainer();
        _ = services.BuildServiceProvider().ApplyMigration();
        return services;
    }
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<SeaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SeaConnection")));

        // _ = services.AddIdentityCore<User>().AddEntityFrameworkStores<SeaDbContext>()/*.AddDefaultTokenProviders()*/;

        _ = services.AddScoped<SeaDbContext>();

        return services;
    }

    private static IServiceCollection AddServicesToContainer(this IServiceCollection services)
    {
        // Register the generic repository
        _ = services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        _ = services.AddScoped(typeof(ISeaDbContextFactory<,>), typeof(SeaDbContextFactory<,>));
        _ = services.AddScoped(typeof(IUnitOfWork<,>), typeof(UnitOfWork<,>));

        return services;

    }

    private static IServiceProvider ApplyMigration(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SeaDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        return services;
    }

}
