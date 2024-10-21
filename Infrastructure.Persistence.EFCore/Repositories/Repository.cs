using EFCore.BulkExtensions;
using Infrastructure.Persistence.EF.Extensions;
using Infrastructure.Persistence.EFCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Sea.Domain.Entities;
using Sea.Domain.Extensions.PaginationHelper;
using Sea.Domain.Repositories;
using System.Linq.Expressions;
using System.Reflection;
using static System.Text.Json.JsonSerializer;

namespace Infrastructure.Persistence.EF.Repositories;

public class Repository<TEntity, TKey>(ILogger<Repository<TEntity, TKey>> _logger, ISeaDbContextFactory<TEntity, TKey> _seaDbContextFactory) : IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    private readonly DbContext _dbContext = _seaDbContextFactory._dbContext ?? throw new AggregateException();
    public async ValueTask<TEntity> GetById(TKey id)
    {
        try
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            LogHelper.LogInformation("[RepositoryBase] [{TEntity}] [GetById] {Id} message : {ex}", typeof(TEntity), id, ex.Message);
            return default;
        }
    }

    public async ValueTask<TEntity> GetByIdAsync(TKey id, bool isAsNoTracking = true)
    {
        try
        {
            return isAsNoTracking
                ? await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(d => EqualityComparer<TKey>.Default.Equals(d.Id, id))
                : await _dbContext.Set<TEntity>().FirstOrDefaultAsync(d => EqualityComparer<TKey>.Default.Equals(d.Id, id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetByIdAsync-Exception: {Id}", id);
            throw;
        }
    }

    public async ValueTask<TEntity> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking();

            if (includes == null)
            {
                return await query.FirstOrDefaultAsync(d => EqualityComparer<TKey>.Default.Equals(d.Id, id));
            }

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync(d => EqualityComparer<TKey>.Default.Equals(d.Id, id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetByIdAsync-Exception: {Id}", id);
            throw;
        }
    }

    public async ValueTask<IEnumerable<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            return await FilterQuery(predicate, orderBy, includes).ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetListAsync-Exception: {predicate} - {includes} - {orderBy}", predicate, includes, orderBy);
            throw;
        }
    }

    public async ValueTask<PaginatedList<TEntity>> GetListAsync(
        int? pageIndex, int? pageSize,
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            return await FilterQuery(pageIndex, pageSize, predicate, orderBy, includes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetListAsync-Exception: {pageIndex} - {pageSize} - {predicate} - {includes} - {orderBy}", pageIndex, pageSize, predicate, includes, orderBy);
            throw;
        }
    }

    public async ValueTask InsertAsync(TKey? userId, TEntity entity)
    {
        try
        {
            entity.CreatedBy = userId;
            entity.ModifiedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            _ = await _dbContext.Set<TEntity>().AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-InsertAsync-Exception: {userId} - {entity}", userId, Serialize(entity));
            throw;
        }
    }

    public async ValueTask InsertAsync(TKey? userId, ICollection<TEntity> entities)
    {
        try
        {
            foreach (var item in entities)
            {
                item.CreatedBy = userId;
                item.ModifiedBy = userId;
                item.CreatedDate = DateTime.UtcNow;
                item.ModifiedDate = DateTime.UtcNow;
                item.IsActive = true;
                item.IsDeleted = false;
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-InsertAsync-Exception: {userId} - {entities}", userId, Serialize(entities));
            throw;
        }
    }

    public void Delete(TEntity entity)
    {
        try
        {
            _ = _dbContext.Set<TEntity>().Remove(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Delete-Exception: {entity}", Serialize(entity));
            throw;
        }
    }

    public void Delete(ICollection<TEntity> entities)
    {
        try
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Delete-Exception: {entities}", Serialize(entities));
            throw;
        }
    }

    public void Update(TKey? userId, TEntity entity)
    {
        try
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = userId;

            var entry = _dbContext.Set<TEntity>().Attach(entity);
            entry.State = EntityState.Modified;

            entry.Property(e => e.CreatedBy).IsModified = false;
            entry.Property(e => e.CreatedDate).IsModified = false;
            entry.Property(e => e.IsDeleted).IsModified = false;

            var properties = typeof(TEntity).GetProperties();
            foreach (var propertyInfo in properties)
            {
                //var noUpdateAttribute = propertyInfo.GetCustomAttribute<IgnoreUpdateAttribute>();
                //if (noUpdateAttribute != null)
                //{
                //    entry.Property(propertyInfo.Name).IsModified = false;
                //}
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Update-Exception: {userId} - {entity}", userId, Serialize(entity));
            throw;
        }
    }

    public void Update(TKey? userId, ICollection<TEntity> entities)
    {
        try
        {
            foreach (var item in entities)
            {
                item.ModifiedDate = DateTime.UtcNow;
                item.ModifiedBy = userId;

                var entry = _dbContext.Set<TEntity>().Attach(item);
                entry.State = EntityState.Modified;

                entry.Property(e => e.CreatedBy).IsModified = false;
                entry.Property(e => e.CreatedDate).IsModified = false;
                entry.Property(e => e.IsDeleted).IsModified = false;

                var properties = typeof(TEntity).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    //var noUpdateAttribute = propertyInfo.GetCustomAttribute<IgnoreUpdateAttribute>();
                    //if (noUpdateAttribute != null)
                    //{
                    //    entry.Property(propertyInfo.Name).IsModified = false;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Update-Exception: {userId} - {entities}", userId, Serialize(entities));
            throw;
        }
    }

    public void Modify(TKey? userId, TEntity entity, params Expression<Func<TEntity, object>>[] properties)
    {
        try
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = userId;

            var entry = _dbContext.Set<TEntity>().Attach(entity);

            entry.Property(e => e.ModifiedDate).IsModified = true;
            entry.Property(e => e.ModifiedBy).IsModified = true;

            foreach (var property in properties)
            {
                entry.Property(property).IsModified = true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Modify-Exception: {userId} - {entity}", userId, Serialize(entity));
            throw;
        }
    }

    public void Modify(TKey? userId, ICollection<TEntity> entities, params Expression<Func<TEntity, object>>[] properties)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.ModifiedDate = DateTime.UtcNow;
                entity.ModifiedBy = userId;

                var entry = _dbContext.Set<TEntity>().Attach(entity);
                var entityType = entity.GetType();

                entry.Property(e => e.ModifiedDate).IsModified = true;
                entry.Property(e => e.ModifiedBy).IsModified = true;

                foreach (var property in properties)
                {
                    entry.Property(property).IsModified = true;
                }
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-Modify-Exception: {userId} - {entities}", userId, Serialize(entities));
            throw;
        }
    }

    public async ValueTask BulkInsertAsync(TKey? userId, ICollection<TEntity> entities)
    {
        try
        {
            foreach (var item in entities)
            {
                item.CreatedDate = DateTime.UtcNow;
                item.CreatedBy = userId;
                item.ModifiedBy = userId;
                item.ModifiedDate = DateTime.UtcNow;
                item.IsActive = true;
                item.IsDeleted = false;
            };
            await _dbContext.BulkInsertAsync(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-BulkInsertAsync-Exception: {userId} - {entities}", userId, Serialize(entities));
            throw;
        }
    }

    public async ValueTask BulkUpdateAsync(TKey? userId, ICollection<TEntity> entities)
    {
        try
        {
            foreach (var item in entities)
            {
                item.ModifiedBy = userId;
                item.ModifiedDate = DateTime.UtcNow;
            };

            await _dbContext.BulkUpdateAsync(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-BulkUpdateAsync-Exception: {userId} - {entities}", userId, Serialize(entities));
            throw;
        }
    }

    public async ValueTask BulkDeleteAsync(ICollection<TEntity> entities)
    {
        try
        {
            await _dbContext.BulkDeleteAsync(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-BulkDeleteAsync-Exception: {entities}", Serialize(entities));
            throw;
        }
    }

    public async ValueTask SoftDeleteAsync(TKey? userId, TKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var entity = await FilterQuery(x => EqualityComparer<TKey>.Default.Equals(x.Id, id), null, includes).FirstOrDefaultAsync() ?? default /*throw new AbandonedMutexException(_localizer["Error.DataNotFound"]).WithData("id", id)*/;
            //entity.DeletedDate = DateTime.UtcNow;
            //entity.DeletedBy = userId;
            entity.IsDeleted = true;

            _ = _dbContext.Set<TEntity>().Update(entity);

            foreach (var item in includes)
            {
                var relatedEntities = GetRelatedEntities(entity, item);

                foreach (var relatedEntity in relatedEntities)
                {
                    // relatedEntity.DeletedDate = DateTime.UtcNow;
                    //relatedEntity.DeletedBy = userId;
                    relatedEntity.IsDeleted = true;

                    _ = _dbContext.Update(relatedEntity);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Repository-SoftDeleteAsync-Exception: {Id}", id);

            throw;
        }
    }

    public async ValueTask<TEntity> GetDataSyncToRedisAsync(TKey id)
    {
        try
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(d => EqualityComparer<TKey>.Default.Equals(d.Id, id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetDataSyncToRedisAsync-Exception: {v}", id);
            throw;
        }
    }

    public async ValueTask<IEnumerable<TEntity>> GetDataSyncToRedisAsync(IEnumerable<TKey> ids = null)
    {
        try
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive)
                .WhereIf(ids != null, x => ids.Contains(x.Id));

            return await query.ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Repository-GetDataSyncToRedisAsync-Exception: {ids}", Serialize(ids));
            throw;
        }
    }

    private async ValueTask<PaginatedList<TEntity>> FilterQuery(
        int? pageIndex, int? pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy,
        params Expression<Func<TEntity, object>>[] includes) => await FilterQuery(predicate, orderBy, includes).ToPaginatedListAsync(pageIndex, pageSize);

    private IQueryable<TEntity> FilterQuery(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy.Compile().Invoke(query);
        }

        return query;
    }

    private static IEnumerable<EntityBase<TKey>> GetRelatedEntities(TEntity entity, Expression<Func<TEntity, object>> relatedEntities)
    {
        var memberExpression = relatedEntities.Body as MemberExpression;
        var propertyInfo = memberExpression.Member as PropertyInfo;

        return propertyInfo.GetValue(entity) switch
        {
            IEnumerable<EntityBase<TKey>> collection => collection,
            EntityBase<TKey> singleEntity => [singleEntity],
            _ => []
        };
    }
}