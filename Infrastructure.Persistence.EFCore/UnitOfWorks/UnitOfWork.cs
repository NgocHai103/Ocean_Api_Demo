using Infrastructure.Persistence.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Sea.Domain.Entities;
using Sea.Domain.Repositories;
using Sea.Domain.UnitOfWorks;

namespace Infrastructure.Persistence.EF.UnitOfWorks;
public class UnitOfWork<TEntity, TKey>(IServiceProvider _serviceProvider, ISeaDbContextFactory<TEntity, TKey> _seaDbContextFactory) : IUnitOfWork<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    private IDbContextTransaction _transaction;

    private readonly DbContext _context = _seaDbContextFactory._dbContext ?? throw new NotImplementedException("Couldn't load dbContext");

    IRepository<TEntity, TKey> IUnitOfWork<TEntity, TKey>.Repository => _serviceProvider.GetRequiredService<IRepository<TEntity, TKey>>() ?? throw new NotImplementedException();

    private bool _disposed = false;

    public int SaveChanges() => _context.SaveChanges();

    public async ValueTask<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void BeginTransaction() => _transaction ??= _context.Database.BeginTransaction();

    public async ValueTask BeginTransactionAsync() => _transaction ??= await _context.Database.BeginTransactionAsync();

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _transaction = null;
    }

    public void RollBackTransaction()
    {
        _transaction?.Rollback();
        _transaction = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        _disposed = true;
    }
}
