using Dapper;
using static Dapper.SqlBuilder;

namespace Infrastructure.Persistence.Dapper.Core.Builder;

public class DapperSqlBuilder
{
    private readonly SqlBuilder _builder = new();
    private readonly Template _sqlTemplate;

    public DapperSqlBuilder(SqlTemplateBuilder sqlTemplateBuilder) => _sqlTemplate = _builder.AddTemplate(sqlTemplateBuilder.ToString());

    public string RawSql => _sqlTemplate.RawSql;

    public object Parameters => _sqlTemplate.Parameters;

    public SqlBuilder InnerJoin(string sql, dynamic? parameters = null) => _builder.InnerJoin(sql, parameters);

    public SqlBuilder LeftJoin(string sql, dynamic? parameters = null) => _builder.LeftJoin(sql, parameters);

    public SqlBuilder RightJoin(string sql, dynamic? parameters = null) => _builder.RightJoin(sql, parameters);

    public SqlBuilder Where(string sql, dynamic? parameters = null) => _builder.Where(sql, parameters);

    public SqlBuilder OrWhere(string sql, dynamic? parameters = null) => _builder.OrWhere(sql, parameters);

    public SqlBuilder OrderBy(string sql, dynamic? parameters = null) => _builder.OrderBy(sql, parameters);

    public SqlBuilder Select(string sql, dynamic? parameters = null) => _builder.Select(sql, parameters);

    public SqlBuilder AddParameters(dynamic? parameters) => _builder.AddParameters(parameters);

    public SqlBuilder Join(string sql, dynamic? parameters = null) => _builder.Join(sql, parameters);

    public SqlBuilder GroupBy(string sql, dynamic? parameters = null) => _builder.GroupBy(sql, parameters);

    public SqlBuilder Having(string sql, dynamic? parameters = null) => _builder.Having(sql, parameters);

    public SqlBuilder Set(string sql, dynamic? parameters = null) => _builder.Set(sql, parameters);
}