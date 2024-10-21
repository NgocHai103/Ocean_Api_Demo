using Infrastructure.Persistence.Dapper.Core.Attributes;
using Infrastructure.Persistence.Dapper.Core.Builder;
using Infrastructure.Persistence.Dapper.Core.Builder.Abstraction;
using Sea.Domain.PGCampusMails;

namespace Infrastructure.Persistence.Dapper.DbContexts.Configuration.PgCampusMail;

[DapperDbContextType(typeof(TestDbContext))]
internal class ICSMailConfiguration : IEntityTypeConfiguration<ICSMail>
{
    public void Configure(EntityTypeBuilder<ICSMail> builder)
    {
        builder.ToTable("ICSMail");

        builder.Property(x => x.Id)
            .HasColumnName("MailId")
            .IsKey();
    }
}
