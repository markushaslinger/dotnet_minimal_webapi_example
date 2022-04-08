using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalBackend.Migrations
{
    public partial class RubbleMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RubbleMenus",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MenuGroupName = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MenuType = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubbleMenus", x => x.MenuId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RubbleMenus_Date_Location",
                table: "RubbleMenus",
                columns: new[] { "Date", "Location" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RubbleMenus");
        }
    }
}
