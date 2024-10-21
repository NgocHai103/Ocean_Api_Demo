using Infrastructure.Persistence.Dapper.Core.Extentions;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Dapper.Core.Builder;
internal interface IEntityTypeBuilder
{
    IEntityTypeBuilder ToTable(string name);
    IEntityTypeBuilder HasSchema(string schema);
}
internal class EntityTypeBuilder<TEntity> : IEntityTypeBuilder
    where TEntity : class
{
    public EntityMetaData<TEntity> MetaData { get; }

    public EntityTypeBuilder() => MetaData = new EntityMetaData<TEntity>();

    public IEntityTypeBuilder ToTable(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new NullReferenceException("Table Entity Mapping is incorrect. Name is required");
        }

        MetaData.TableName = name;

        return this;
    }

    public IEntityTypeBuilder HasSchema(string schema)
    {
        if (string.IsNullOrEmpty(schema))
        {
            throw new NullReferenceException("schema Mapping is incorrect. Schema is required");
        }

        MetaData.Schema = schema;

        return this;
    }

    public PropertyBuilder Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        string memberName = propertyExpression.GetMemberName();

        var propertyBuilder = new PropertyBuilder(memberName);

        if (!MetaData.MembersMetaData.ContainsKey(memberName))
        {
            MetaData.MembersMetaData.Add(propertyBuilder.MetaData.Name, propertyBuilder.MetaData);
        }
        else
        {
            propertyBuilder.MetaData = MetaData.MembersMetaData[memberName];
        }

        return propertyBuilder;

    }
}