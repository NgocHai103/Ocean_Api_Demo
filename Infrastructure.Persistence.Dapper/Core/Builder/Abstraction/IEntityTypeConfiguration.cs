namespace Infrastructure.Persistence.Dapper.Core.Builder.Abstraction;

internal interface IEntityTypeConfiguration { }

internal interface IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    void Configure(EntityTypeBuilder<TEntity> builder);
}
