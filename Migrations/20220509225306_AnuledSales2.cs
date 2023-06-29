using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AnuledSales2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnuladaSales");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Sales");

            migrationBuilder.AddColumn<string>(
                name: "AnulatedById",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AnulatedById",
                table: "SaleDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "SaleDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Sales_AnulatedById",
                table: "Sales",
                column: "AnulatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_AnulatedById",
                table: "SaleDetails",
                column: "AnulatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_AspNetUsers_AnulatedById",
                table: "SaleDetails",
                column: "AnulatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_AnulatedById",
                table: "Sales",
                column: "AnulatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_AspNetUsers_AnulatedById",
                table: "SaleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_AnulatedById",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_AnulatedById",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetails_AnulatedById",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "AnulatedById",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "AnulatedById",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "SaleDetails");

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnuladaSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnuledById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false)
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
    }
}
