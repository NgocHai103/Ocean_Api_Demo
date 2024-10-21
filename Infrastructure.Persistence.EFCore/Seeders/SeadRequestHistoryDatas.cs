using Microsoft.EntityFrameworkCore;
using Sea.Domain.Entities.Sea;
using Sea.Domain.Enums;

namespace Infrastructure.Persistence.Dapper.Seeders;
public static partial class SeedDatas
{
    public static void SeedRequestHistory(this ModelBuilder modelBuilder) => _ = modelBuilder.Entity<RequestHistory>().HasData(
           new RequestHistory
           {
               Id = new Guid("ed9ad813-64eb-4676-ac00-11f9c0ddaf39"),
               RequestId = new Guid("ed9ad813-64eb-4676-ac00-11f9c0f9af39"), // Link this to an actual RequestId from Requests
               AssigneeId = Guid.NewGuid(),
               AssigneeName = "Jane Smith",
               Status = RequestStatuses.New,
               Description = "Initial status assigned."
           },
            new RequestHistory
            {
                Id = new Guid("ab23ff99-31dd-46e8-a89a-c4a60ac4fca2"),
                RequestId = new Guid("ab23ff93-31dd-46e8-a89a-c4a60ac4fca2"), // Link this to an actual RequestId from Requests
                AssigneeId = Guid.NewGuid(),
                AssigneeName = "Bob Builder",
                Status = RequestStatuses.InProgress,
                Description = "Moved to in-progress status."
            }
        );
}
