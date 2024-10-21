using Microsoft.EntityFrameworkCore;
using Sea.Domain.Entities;

namespace Infrastructure.Persistence.EFCore.DbContexts;
public interface ISeaDbContextFactory<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    public DbContext _dbContext { get; }
}
