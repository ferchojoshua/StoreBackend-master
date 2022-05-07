using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AnuledSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnulado",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnulado",
                table: "SaleDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AnuladaSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnuledById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnuladaSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnuladaSales_AspNetUsers_AnuledById",
                        column: x => x.AnuledById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnuladaSales_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnuladaSales_AnuledById",
                table: "AnuladaSales",
                column: "AnuledById");

            migrationBuilder.CreateIndex(
                name: "IX_AnuladaSales_SaleId",
                table: "AnuladaSales",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnuladaSales");

            migrationBuilder.DropColumn(
                name: "IsAnulado",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsAnulado",
                table: "SaleDetails");
        }
    }
}
