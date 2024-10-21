using Infrastructure.Persistence.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sea.Domain.Entities;

namespace Infrastructure.Persistence.EF.DbContexts.Core;
public class SeaDbContextFactory<TEntity, TKey>(IServiceProvider _serviceProvider) : ISeaDbContextFactory<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    public DbContext _dbContext => GetDbContextForEntityType();
    private static List<Type> DbContextTypes { get; set; }

    private DbContext GetDbContextForEntityType()
    {
        // Get all registered DbContext types in the project using reflection
        DbContextTypes ??= AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(DbContext)))
            .ToList();

        // Iterate over all DbContext types to find one that has a DbSet<TEntity>
        foreach (var dbContextType in DbContextTypes)
        {
            // Check if the DbContext has a DbSet<TEntity> property
            var dbSetProperty = dbContextType.GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                     p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                     p.PropertyType.GetGenericArguments()[0] == typeof(TEntity));

            if (dbSetProperty != null)
            {
                // Resolve the DbContext instance from the DI container
                return (DbContext)_serviceProvider.GetRequiredService(dbContextType);
            }
        }

        // Throw an exception if no matching DbContext is found
        throw new InvalidOperationException($"No DbContext found for entity type {typeof(TEntity).Name}");
    }
}
