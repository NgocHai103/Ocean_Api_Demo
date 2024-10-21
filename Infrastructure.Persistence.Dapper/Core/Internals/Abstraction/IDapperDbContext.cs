using Infrastructure.Persistence.Dapper.Core.Builder;
using System.Transactions;

namespace Infrastructure.Persistence.Dapper.Core.Internals.Abstraction;

public interface IDapperDbContext : IRawQuery, ICommand
{
    TransactionScope CreateTransactionScope { get; }

    DapperSqlBuilder CreateSqlBuilder(string template);
}
