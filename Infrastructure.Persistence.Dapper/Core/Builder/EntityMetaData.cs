using System.Reflection;

namespace Infrastructure.Persistence.Dapper.Core.Builder;

internal class EntityMetaData
{
    public string? TableName { get; set; }

    public string? Schema { get; set; } = "dbo";

    public PropertyMetaDataCollection MembersMetaData { get; set; } = [];
}
internal class EntityMetaData<TEntity> : EntityMetaData
    where TEntity : class
{
    public EntityMetaData()
    {
        TableName = typeof(TEntity).Name;
        LoadAllMemberMetaData();
    }

    private void LoadAllMemberMetaData()
    {
        var properties = typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => !x.PropertyType.IsGenericType
            && !x.PropertyType.IsClass
            && !x.PropertyType.IsInterface
            && !x.PropertyType.IsAbstract);
        foreach (var property in properties)
        {
            MembersMetaData.Add(property.Name, new PropertyMetaData(property.Name));
        }
    }
}

internal class PropertyMetaData
{
    public string Name { get; init; }

    public string SqlColumnName { get; set; }

    public bool IsKey { get; set; }

    public bool IsIgnore { get; set; }

    public object? DefaultValue { get; set; }

    public Func<object>? GenerateValue { get; set; }

    public PropertyMetaData(string propertyName)
    {
        Name = propertyName;
        SqlColumnName = Name;
    }
}