namespace Infrastructure.Persistence.Dapper.Core.Internals.Abstraction;
public interface ICommand
{
    ValueTask<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class;

    ValueTask<IList<TEntity>> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    ValueTask<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    ValueTask<int> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

}
