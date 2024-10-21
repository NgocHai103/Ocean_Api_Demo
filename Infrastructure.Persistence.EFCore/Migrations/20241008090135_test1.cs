using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumberPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssignedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "AssignedId", "AssignedName", "CategoryId", "CategoryName", "Code", "CreatedBy", "CreatedDate", "CustomerName", "IsActive", "IsDeleted", "ModifiedBy", "ModifiedDate", "NumberPhone", "Status", "SubCategoryId", "SubCategoryName", "Title", "Type" },
                values: new object[,]
                {
                    { new Guid("ab23ff93-31dd-46e8-a89a-c4a60ac4fca2"), new Guid("893ea6b8-a229-4657-b5ba-7b0515fb32ea"), "Bob Builder", new Guid("72fef2df-d5be-4da3-803b-c6f58f5ff4b9"), "Support", "REQ002", new Guid("00000000-0000-0000-0000-000000000000"), null, "Alice Wonderland", false, false, new Guid("00000000-0000-0000-0000-000000000000"), null, "0987654321", 0, new Guid("764bbad2-7c78-4ca4-af68-3df4952b0f1f"), "Sub Support", "Initial Request 2", 0 },
                    { new Guid("ed9ad813-64eb-4676-ac00-11f9c0f9af39"), new Guid("a45a5148-02b0-46a8-9d3e-e96ebfe1a729"), "Jane Smith", new Guid("8ceb3c1f-9cbc-4f5b-a98d-2850273d0acb"), "General", "REQ001", new Guid("00000000-0000-0000-0000-000000000000"), null, "John Doe", false, false, new Guid("00000000-0000-0000-0000-000000000000"), null, "1234567890", 0, new Guid("5d9bfa48-f6b7-42b0-af34-a2cfc71dfdc0"), "Sub General", "Initial Request 1", 1 }
                });

            migrationBuilder.InsertData(
                table: "RequestHistories",
                columns: new[] { "Id", "AssigneeId", "AssigneeName", "CreatedBy", "CreatedDate", "Description", "IsActive", "IsDeleted", "ModifiedBy", "ModifiedDate", "RequestId", "Status" },
                values: new object[,]
                {
                    { new Guid("ab23ff99-31dd-46e8-a89a-c4a60ac4fca2"), new Guid("59c2b667-deb0-49e0-bd73-796e7b5492ae"), "Bob Builder", new Guid("00000000-0000-0000-0000-000000000000"), null, "Moved to in-progress status.", false, false, new Guid("00000000-0000-0000-0000-000000000000"), null, new Guid("ab23ff93-31dd-46e8-a89a-c4a60ac4fca2"), 2 },
                    { new Guid("ed9ad813-64eb-4676-ac00-11f9c0ddaf39"), new Guid("026dc4a3-c551-45c0-abc7-5cd9576a5ab0"), "Jane Smith", new Guid("00000000-0000-0000-0000-000000000000"), null, "Initial status assigned.", false, false, new Guid("00000000-0000-0000-0000-000000000000"), null, new Guid("ed9ad813-64eb-4676-ac00-11f9c0f9af39"), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_RequestId",
                table: "RequestHistories",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestHistories");

            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
