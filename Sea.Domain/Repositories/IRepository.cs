using Sea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sea.Domain.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    public ValueTask<TEntity> GetById(TKey id);

    public ValueTask InsertAsync(TKey? userId, TEntity entity);

    public ValueTask BulkInsertAsync(TKey? userId, ICollection<TEntity> entities);

    public ValueTask InsertAsync(TKey? userId, ICollection<TEntity> entities);
}
