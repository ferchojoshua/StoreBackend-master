using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class UpdateKardex6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductIns_ProductInId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductMovments_ProductMovmentsId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_Sales_SalesId",
                table: "Kardex");

            migrationBuilder.RenameColumn(
                name: "SalesId",
                table: "Kardex",
                newName: "TrasladoInventarioId");

            migrationBuilder.RenameColumn(
                name: "ProductMovmentsId",
                table: "Kardex",
                newName: "SaleId");

            migrationBuilder.RenameColumn(
                name: "ProductInId",
                table: "Kardex",
                newName: "EntradaProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_SalesId",
                table: "Kardex",
                newName: "IX_Kardex_TrasladoInventarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_ProductMovmentsId",
                table: "Kardex",
                newName: "IX_Kardex_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_ProductInId",
                table: "Kardex",
                newName: "IX_Kardex_EntradaProductId");

            migrationBuilder.AddColumn<int>(
                name: "AjusteInventarioId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RealizadoPorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MontoPrecioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockAdjustments_AspNetUsers_RealizadoPorId",
                        column: x => x.RealizadoPorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoFinalCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitarioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoFinalVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockAdjustmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_Productos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_StockAdjustments_StockAdjustmentId",
                        column: x => x.StockAdjustmentId,
                        principalTable: "StockAdjustments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_AjusteInventarioId",
                table: "Kardex",
                column: "AjusteInventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_ProductId",
                table: "StockAdjustmentDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_StockAdjustmentId",
                table: "StockAdjustmentDetails",
                column: "StockAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_StoreId",
                table: "StockAdjustmentDetails",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_RealizadoPorId",
                table: "StockAdjustments",
                column: "RealizadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_StoreId",
                table: "StockAdjustments",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductIns_EntradaProductId",
                table: "Kardex",
                column: "EntradaProductId",
                principalTable: "ProductIns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductMovments_TrasladoInventarioId",
                table: "Kardex",
                column: "TrasladoInventarioId",
                principalTable: "ProductMovments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_Sales_SaleId",
                table: "Kardex",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_StockAdjustments_AjusteInventarioId",
                table: "Kardex",
                column: "AjusteInventarioId",
                principalTable: "StockAdjustments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductIns_EntradaProductId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductMovments_TrasladoInventarioId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_Sales_SaleId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_StockAdjustments_AjusteInventarioId",
                table: "Kardex");

            migrationBuilder.DropTable(
                name: "StockAdjustmentDetails");

            migrationBuilder.DropTable(
                name: "StockAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_AjusteInventarioId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "AjusteInventarioId",
                table: "Kardex");

            migrationBuilder.RenameColumn(
                name: "TrasladoInventarioId",
                table: "Kardex",
                newName: "SalesId");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "Kardex",
                newName: "ProductMovmentsId");

            migrationBuilder.RenameColumn(
                name: "EntradaProductId",
                table: "Kardex",
                newName: "ProductInId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_TrasladoInventarioId",
                table: "Kardex",
                newName: "IX_Kardex_SalesId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_SaleId",
                table: "Kardex",
                newName: "IX_Kardex_ProductMovmentsId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_EntradaProductId",
                table: "Kardex",
                newName: "IX_Kardex_ProductInId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductIns_ProductInId",
                table: "Kardex",
                column: "ProductInId",
                principalTable: "ProductIns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductMovments_ProductMovmentsId",
                table: "Kardex",
                column: "ProductMovmentsId",
                principalTable: "ProductMovments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_Sales_SalesId",
                table: "Kardex",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
