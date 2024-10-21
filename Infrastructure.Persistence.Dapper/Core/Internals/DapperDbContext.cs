using Dapper;
using Infrastructure.Persistence.Dapper.Core.Builder;
using Infrastructure.Persistence.Dapper.Core.Internals.Abstraction;
using Infrastructure.Persistence.Dapper.Extensions;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Infrastructure.Persistence.Dapper.Core.Internals;
public abstract class DapperDbContext(string connectionString/*, IServiceProvider serviceProvider*/) : IDapperDbContext, IAsyncDisposable
{
    //private readonly IMemoryCache _memoryCache = serviceProvider.GetService<IMemoryCache>() ?? throw new ArgumentNullException(nameof(serviceProvider));
    private bool _disposed;

    public IDbConnection DbConnection { get; } = new SqlConnection(connectionString) ?? throw new ArgumentNullException(nameof(connectionString));

    public virtual DapperSqlBuilder CreateSqlBuilder(string template) => string.IsNullOrEmpty(template)
            ? throw new ArgumentNullException(nameof(template), "Template is not null or empty")
            : new DapperSqlBuilder(new SqlTemplateBuilder().FromTemplate(template));

    public TransactionScope CreateTransactionScope => new(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();

        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        DbConnection?.Dispose();

        return ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DbConnection?.Dispose();
        }

        _disposed = true;
    }

    ~DapperDbContext()
    {
        Dispose(false);
    }

    public async ValueTask<IList<TModel>> QueryAsync<TModel>(string query, object? param = null)
    {
        var queryResult = await DbConnection.QueryAsync<TModel>(query, param);
        return queryResult.ToList();
    }

    public async ValueTask<IList<TModel>> QueryAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null)
    {
        var queryResult = await DbConnection.QueryAsync(query, map, param: param, splitOn: splitOn);
        return queryResult.ToList();
    }

    public async ValueTask<TModel> SingleOrDefaltAsync<TModel>(string query, object? param = null)
    {
        var queryResult = await DbConnection.QuerySingleOrDefaultAsync<TModel>(query, param);
        return queryResult;
    }

    public async ValueTask<TModel> SingleOrDefaltAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null)
    {
        var queryResult = await QueryAsync(query, map, param: param, splitOn: splitOn);

        return queryResult.SingleOrDefault() ?? throw new ArgumentException("");
    }

    public async ValueTask<IList<TModel>> ExecuteStoreProcedure<TModel>(string storeProcedureName, params SqlParameter[] sqlParameters)
    {
        var query = GetFunctionOrStoreProcedureQuery(storeProcedureName, sqlParameters);
        var queryResult = await QueryAsync<TModel>(query, sqlParameters);
        return queryResult;
    }

    public async ValueTask<TModel> ExecuteSqlFunction<TModel>(string sqlFunctionName, params SqlParameter[] sqlParameters)
    {
        var query = GetFunctionOrStoreProcedureQuery(sqlFunctionName, sqlParameters);
        var queryResult = await DbConnection.ExecuteScalarAsync<TModel>(query, sqlParameters);
        return queryResult;
    }

    public async ValueTask<int> ExecuteQuery(string sqlQuery, object? param = null) => await DbConnection.ExecuteAsync(sqlQuery, param);

    private string GetFunctionOrStoreProcedureQuery(string sqlFunctionName, params SqlParameter[] sqlParameters) => $"exec {sqlFunctionName} {string.Join(',', sqlParameters.Select(x => $"@{x.ParameterName}"))}";

    public async ValueTask<TModel> FirstOrDefaltAsync<TModel>(string query, object? param = null)
    {
        var result = await DbConnection.QueryFirstAsync<TModel>(query, param);
        return result;
    }

    public async ValueTask<TModel> FirstOrDefaltAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null)
    {
        var queryResult = await QueryAsync(query, map, param: param, splitOn: splitOn);

        return queryResult.FirstOrDefault() ?? throw new ArgumentException("");
    }

    public async ValueTask<int> CountAsync(string sqlQuery, object? param = null)
    {
        var query = $"SELECT Count(1) From ({sqlQuery}) t1";
        return await DbConnection.ExecuteScalarAsync<int>(query, param);
    }

    public async ValueTask<IList<TModel>> ExecuteStoreProcedure<TParent, TChild, TModel>(string storeProcedureName, SqlParameter[] sqlParameters, Func<TParent, TChild, TModel> map, string splitOn)
    {
        var query = GetFunctionOrStoreProcedureQuery(storeProcedureName, sqlParameters);
        var queryResult = await QueryAsync(query, map, param: sqlParameters, splitOn: splitOn);
        return queryResult;
    }

    public ValueTask<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var (RawSql, sqlParameters) = entity.ToInsertSQLCommand(this);

        return SingleOrDefaltAsync<TEntity>(RawSql, sqlParameters);
    }

    public ValueTask<IList<TEntity>> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        var (RawSql, sqlParameters) = entities.ToInsertRangeSQLCommand(this);

        return QueryAsync<TEntity>(RawSql, sqlParameters);
    }

    public ValueTask<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var (RawSql, sqlParameters) = entity.ToUpdateSQLCommand(this);

        return ExecuteQuery(RawSql, sqlParameters);
    }

    public ValueTask<int> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var (RawSql, sqlParameters) = entity.ToDeleteSQLCommand(this);

        return ExecuteQuery(RawSql, sqlParameters);
    }
}
