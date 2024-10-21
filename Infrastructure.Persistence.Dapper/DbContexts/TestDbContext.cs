using Infrastructure.Persistence.Dapper.Core.Internals;

namespace Infrastructure.Persistence.Dapper.DbContexts;

public sealed class TestDbContext(string connectionString) : DapperDbContext(connectionString)
{
}
