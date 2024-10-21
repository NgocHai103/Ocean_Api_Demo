using Sea.Domain.Entities;
using Sea.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Sea.Domain.UnitOfWorks;
public interface IUnitOfWork<TEntity, TKey> : IDisposable where TEntity : EntityBase<TKey>
{
    public IRepository<TEntity, TKey> Repository { get; }

    public int SaveChanges();

    public ValueTask<int> SaveChangesAsync();

    public void BeginTransaction();

    public ValueTask BeginTransactionAsync();

    public void CommitTransaction();

    public void RollBackTransaction();
}
