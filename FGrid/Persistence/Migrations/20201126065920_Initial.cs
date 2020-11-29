using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGrid.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Address_Street = table.Column<string>(nullable: true),
                    Address_ZipCode = table.Column<string>(nullable: true),
                    Address_Country = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
