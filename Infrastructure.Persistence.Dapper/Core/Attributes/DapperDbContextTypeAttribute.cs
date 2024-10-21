using Infrastructure.Persistence.Dapper.Core.Internals;

namespace Infrastructure.Persistence.Dapper.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal class DapperDbContextTypeAttribute : Attribute
{
    public Type DbContextType { get; }

    public DapperDbContextTypeAttribute(Type type)
    {
        if (type.BaseType != typeof(DapperDbContext))
        {
            throw new ArgumentException("Invalid type. It should be the derived class of IDapperDbContext");
        }

        DbContextType = type;
    }
}