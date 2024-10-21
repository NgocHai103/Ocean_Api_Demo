using Microsoft.EntityFrameworkCore;
using Sea.Domain.Entities.Sea;
using Sea.Domain.Enums;

namespace Infrastructure.Persistence.Dapper.Seeders;
public static partial class SeedDatas
{
    public static void SeedRequest(this ModelBuilder modelBuilder) => _ = modelBuilder.Entity<Request>().HasData(
            new Request
            {
                Id = new Guid("ed9ad813-64eb-4676-ac00-11f9c0f9af39"),
                Title = "Initial Request 1",
                CustomerName = "John Doe",
                NumberPhone = "1234567890",
                Code = "REQ001",
                Status = RequestStatuses.New,
                Type = RequestTypes.Request,
                CategoryId = Guid.NewGuid(),
                CategoryName = "General",
                SubCategoryId = Guid.NewGuid(),
                SubCategoryName = "Sub General",
                AssignedId = Guid.NewGuid(),
                AssignedName = "Jane Smith"
            },
            new Request
            {
                Id = new Guid("ab23ff93-31dd-46e8-a89a-c4a60ac4fca2"),
                Title = "Initial Request 2",
                CustomerName = "Alice Wonderland",
                NumberPhone = "0987654321",
                Code = "REQ002",
                Status = RequestStatuses.New,
                Type = RequestTypes.Case,
                CategoryId = Guid.NewGuid(),
                CategoryName = "Support",
                SubCategoryId = Guid.NewGuid(),
                SubCategoryName = "Sub Support",
                AssignedId = Guid.NewGuid(),
                AssignedName = "Bob Builder"
            }
        );
}
