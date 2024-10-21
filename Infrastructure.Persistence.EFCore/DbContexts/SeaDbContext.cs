using Infrastructure.Persistence.Dapper.Seeders;
using Microsoft.EntityFrameworkCore;
using Sea.Domain.Entities.Sea;

namespace Infrastructure.Persistence.EF.DbContexts;

public class SeaDbContext(DbContextOptions<SeaDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SeedRequest();
        builder.SeedRequestHistory();
    }
    public DbSet<Request> Requests { get; set; }

    public DbSet<RequestHistory> RequestHistories { get; set; }
}

