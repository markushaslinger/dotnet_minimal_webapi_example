using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalBackend.Migrations
{
    public partial class RubbleOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RubbleOrders",
                columns: table => new
                {
                    OrderNo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderingEmployee = table.Column<string>(type: "TEXT", nullable: false),
                    MenuId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubbleOrders", x => x.OrderNo);
                    table.ForeignKey(
                        name: "FK_RubbleOrders_RubbleMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "RubbleMenus",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RubbleOrders_MenuId",
                table: "RubbleOrders",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RubbleOrders");
        }
    }
}
