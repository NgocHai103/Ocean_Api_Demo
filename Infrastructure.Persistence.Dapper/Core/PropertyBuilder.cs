using Infrastructure.Persistence.Dapper.Core.Builder;

namespace Infrastructure.Persistence.Dapper.Core;
internal class PropertyBuilder(string name)
{
    public PropertyMetaData MetaData { get; set; } = new PropertyMetaData(name);

    public PropertyBuilder HasColumnName(string sqlColumnName)
    {
        MetaData.SqlColumnName = sqlColumnName;
        return this;
    }

    public PropertyBuilder IgnoreBinding()
    {
        MetaData.IsIgnore = true;
        return this;
    }

    public PropertyBuilder HasGenerateValue(Func<object> func)
    {
        MetaData.GenerateValue = func;
        return this;
    }

    public PropertyBuilder HasDefaultValue(object defaultValue)
    {
        MetaData.DefaultValue = defaultValue;
        return this;
    }

    public PropertyBuilder IsKey()
    {
        MetaData.IsKey = true;
        return this;
    }
}
