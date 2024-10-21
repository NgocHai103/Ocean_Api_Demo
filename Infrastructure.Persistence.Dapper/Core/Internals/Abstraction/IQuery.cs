using System.Data.SqlClient;

namespace Infrastructure.Persistence.Dapper.Core.Internals.Abstraction;
public interface IRawQuery
{
    ValueTask<IList<TModel>> QueryAsync<TModel>(string query, object? param = null);

    ValueTask<IList<TModel>> QueryAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null);

    ValueTask<TModel> SingleOrDefaltAsync<TModel>(string query, object? param = null);

    ValueTask<TModel> SingleOrDefaltAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null);

    ValueTask<TModel> FirstOrDefaltAsync<TModel>(string query, object? param = null);

    ValueTask<TModel> FirstOrDefaltAsync<TParent, TChild, TModel>(string query, Func<TParent, TChild, TModel> map, string splitOn, object? param = null);

    ValueTask<IList<TModel>> ExecuteStoreProcedure<TModel>(string storeProcedureName, params SqlParameter[] sqlParameters);

    ValueTask<TModel> ExecuteSqlFunction<TModel>(string sqlFunctionName, params SqlParameter[] sqlParameters);

    ValueTask<IList<TModel>> ExecuteStoreProcedure<TParent, TChild, TModel>(string storeProcedureName, SqlParameter[] sqlParameters, Func<TParent, TChild, TModel> map, string splitOn);

    ValueTask<int> CountAsync(string sqlQuery, object? param = null);

    ValueTask<int> ExecuteQuery(string sqlQuery, object? param = null);
}
