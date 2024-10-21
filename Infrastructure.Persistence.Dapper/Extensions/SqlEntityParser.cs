using Infrastructure.Persistence.Dapper.Core.Builder;
using Infrastructure.Persistence.Dapper.Core.Internals;
using System.Data.SqlClient;
using System.Text;

namespace Infrastructure.Persistence.Dapper.Extensions;

internal static class SqlEntityParser
{
    public static (string RawSql, SqlParameter[] sqlParameters) ToInsertSQLCommand<TEntity>(this TEntity entity, DapperDbContext context) where TEntity : class
    {
        var metaData = InternalEntitiesMapping.GetEntityMetaData(typeof(TEntity), context.GetType());

        var columns = metaData?.MembersMetaData.Values.Where(x => !x.IsIgnore).ToList();
        var sqlComlumnName = columns is { Count: > 0 } ? string.Join(',', columns.Select(x => x.SqlColumnName)) : string.Empty;
        var sqlParameterNames = columns is { Count: > 0 } ? string.Join(',', columns.Select(x => $"@{x.SqlColumnName}")) : string.Empty;
        var rawSql = $"INSERT INTO {context.DbConnection.Database}.{metaData?.Schema}.{metaData?.TableName}({sqlComlumnName}) OUTPUT INSERTED.* VALUES({sqlParameterNames})";

        var sqlParameter = columns?.Select(x =>
        {
            if (x.GenerateValue != null)
            {
                var value = x.GenerateValue.Invoke();
                return new SqlParameter(x.SqlColumnName, value);
            }

            var propertyValue = GetPropertyEntityValue(entity, x);

            return new SqlParameter(x.SqlColumnName, propertyValue);
        }).ToArray();

        return (rawSql, sqlParameter ?? []);
    }

    public static (string RawSql, SqlParameter[] sqlParameters) ToInsertRangeSQLCommand<TEntity>(this IEnumerable<TEntity> entities, DapperDbContext context) where TEntity : class
    {
        var updateEntities = entities.ToList();
        var metaData = InternalEntitiesMapping.GetEntityMetaData(typeof(TEntity), context.GetType());

        var columns = metaData?.MembersMetaData.Values.Where(x => !x.IsIgnore).ToList();
        var sqlComlumnName = columns is { Count: > 0 } ? string.Join(',', columns.Select(x => x.SqlColumnName)) : string.Empty;

        var rawSql = new StringBuilder($"INSERT INTO {context.DbConnection.Database}.{metaData?.Schema}.{metaData?.TableName}({sqlComlumnName}) OUTPUT INSERTED.* ");
        var parameters = new List<SqlParameter>();
        for (var i = 0; i < updateEntities.Count; i++)
        {
            var entity = updateEntities[i];
            var sqlParameterNames = columns is { Count: > 0 } ? columns.Select(x => new SqlParameter($"@{x.SqlColumnName}{i + 1}", x.GenerateValue != null ? x.GenerateValue.Invoke() : GetPropertyEntityValue(updateEntities[i], x))) : [];
            _ = rawSql.AppendLine($"VALUES({string.Join(',', sqlParameterNames.Select(x => x.ParameterName))}");
            parameters.AddRange(sqlParameterNames);
        }

        return (rawSql.ToString(), parameters.ToArray());
    }

    public static (string RawSql, SqlParameter[] sqlParameters) ToUpdateSQLCommand<TEntity>(this TEntity entity, DapperDbContext context) where TEntity : class
    {
        var metaData = InternalEntitiesMapping.GetEntityMetaData(typeof(TEntity), context.GetType());

        var columns = metaData?.MembersMetaData.Values.Where(x => !x.IsIgnore && !x.IsKey).ToList();
        var keyColumn = metaData?.MembersMetaData.Values.SingleOrDefault(x => x.IsKey);

        var sqlParameterNames = columns is { Count: > 0 } ? string.Join(',', columns.Where(x => !x.IsKey).Select(x => $"p.{x.SqlColumnName} = @{x.SqlColumnName}")) : string.Empty;

        var rawSql = $"UPDATE {context.DbConnection.Database}.{metaData?.Schema}.{metaData?.TableName} p SET {sqlParameterNames} WHERE p.{keyColumn?.SqlColumnName} = @{keyColumn?.SqlColumnName}";

        var sqlParameter = columns?.Select(x =>
        {
            var propertyValue = GetPropertyEntityValue(entity, x);

            return new SqlParameter(x.SqlColumnName, propertyValue);
        }).ToArray();

        return (rawSql, sqlParameter ?? []);
    }

    public static (string RawSql, SqlParameter[] sqlParameters) ToDeleteSQLCommand<TEntity>(this TEntity entity, DapperDbContext context) where TEntity : class
    {
        var metaData = InternalEntitiesMapping.GetEntityMetaData(typeof(TEntity), context.GetType());
        var keyColumn = metaData?.MembersMetaData.Values.SingleOrDefault(x => x.IsKey);

        var rawSql = $"DELETE FROM {context.DbConnection.Database}.{metaData?.Schema}.{metaData?.TableName} p WHERE p.{keyColumn?.SqlColumnName} = @{keyColumn?.SqlColumnName}";
        var propertyValue = GetPropertyEntityValue(entity, keyColumn);

        return (rawSql, new[] { new SqlParameter($"@{keyColumn?.SqlColumnName}", propertyValue) });
    }

    private static object GetPropertyEntityValue<TEntity>(TEntity entity, PropertyMetaData? keyColumn) where TEntity : class
    {
        var propertyInfo = entity.GetType().GetProperty(keyColumn?.Name ?? string.Empty);
        var propertyValue = propertyInfo?.GetValue(entity);
        return propertyValue ?? new();
    }
}
