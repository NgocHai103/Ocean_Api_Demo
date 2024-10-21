using Infrastructure.Persistence.Dapper.Core.Attributes;
using Infrastructure.Persistence.Dapper.Core.Builder;
using Infrastructure.Persistence.Dapper.Core.Builder.Abstraction;
using Infrastructure.Persistence.Dapper.Core.Internals.Abstraction;
using System.Collections.Concurrent;
using System.Reflection;

namespace Infrastructure.Persistence.Dapper.Core.Internals;
internal class InternalEntitiesMapping
{
    public static readonly ConcurrentDictionary<Type, Dictionary<Type, EntityMetaData>> InternalEntityConfigurations = RegisterMapping();

    public static EntityMetaData? GetEntityMetaData(Type entityType, Type dbContextType)
    {
        if (InternalEntityConfigurations.TryGetValue(dbContextType, out var iEConfigurtion))
        {
            var metaDataConfiguration = iEConfigurtion;
            if (metaDataConfiguration.TryGetValue(entityType, out var mConfigurtion))
            {
                return mConfigurtion;
            }
        }

        var metaData = Activator.CreateInstance(typeof(EntityMetaData<>).MakeGenericType(entityType));

        return metaData as EntityMetaData;
    }

    private static ConcurrentDictionary<Type, Dictionary<Type, EntityMetaData>> RegisterMapping()
    {
        var configurations = new ConcurrentDictionary<Type, Dictionary<Type, EntityMetaData>>();

        var assembly = Assembly.GetAssembly(typeof(IDapperDbContext));
        var dbContextEntityTypeConfigurations = assembly?.GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && IsEntityTypeConfiguration(t))
            .SelectMany(t => t.GetCustomAttributes<DapperDbContextTypeAttribute>(false),
            (t, att) => new
            {
                EntityTypeConfiguration = Activator.CreateInstance(t),
                att.DbContextType,
                GenericType = t.GetInterface(typeof(IEntityTypeConfiguration<>).Name)?.GetGenericArguments().FirstOrDefault()
            })
            .GroupBy(x => x.DbContextType)
            .Select(g => new
            {
                DbContextType = g.Key,
                Configurations = g.Select(a => new
                {
                    a.EntityTypeConfiguration,
                    a.GenericType
                })
            });

        foreach (var configureItem in dbContextEntityTypeConfigurations ?? [])
        {
            var entityMetaDatas = new Dictionary<Type, EntityMetaData>();
            foreach (var item in configureItem.Configurations)
            {
                var entityBuilder = item?.GenericType != null
                                    ? Activator.CreateInstance(typeof(EntityTypeBuilder<>).MakeGenericType(item.GenericType))
                                    : throw new InvalidOperationException("GenericType cannot be null");

                var configureMethodInfo = item.EntityTypeConfiguration?.GetType().GetMethod(nameof(IEntityTypeConfiguration<object>.Configure));

                _ = (configureMethodInfo?.Invoke(item.EntityTypeConfiguration, [entityBuilder ?? new()]));

                var metaDataPropertyInfo = entityBuilder?.GetType().GetProperty(nameof(EntityTypeBuilder<object>.MetaData));
                entityMetaDatas.Add(item.GenericType, (metaDataPropertyInfo?.GetValue(entityBuilder) as EntityMetaData) ?? new());
            }

            _ = configurations.TryAdd(configureItem.DbContextType, entityMetaDatas);
        }

        return configurations;
    }

    private static bool IsEntityTypeConfiguration(Type type) => type.GetInterfaces().Any(x => x.IsGenericType && x.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.OrdinalIgnoreCase));

}
