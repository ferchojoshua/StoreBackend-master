using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacturedById",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVenta",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsEventual",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoVenta",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SaleDetailId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descuento = table.Column<int>(type: "int", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PVM = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PVD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleDetails_Productos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_FacturedById",
                table: "Sales",
                column: "FacturedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleDetailId",
                table: "Sales",
                column: "SaleDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_StoreId",
                table: "SaleDetails",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_FacturedById",
                table: "Sales",
                column: "FacturedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleDetails_SaleDetailId",
                table: "Sales",
                column: "SaleDetailId",
                principalTable: "SaleDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_FacturedById",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleDetails_SaleDetailId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropIndex(
                name: "IX_Sales_FacturedById",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleDetailId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FacturedById",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FechaVenta",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsEventual",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "MontoVenta",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleDetailId",
                table: "Sales");
        }
    }
}
