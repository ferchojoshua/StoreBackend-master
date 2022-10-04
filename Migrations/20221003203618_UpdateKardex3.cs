using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class UpdateKardex3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductIns_EntradaProductoId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductMovments_TrasladoInventarioId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_SaleAnulations_SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_Sales_SaleId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_EntradaProductoId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_SaleId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "EntradaProductoId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Kardex");

            migrationBuilder.RenameColumn(
                name: "TrasladoInventarioId",
                table: "Kardex",
                newName: "ProductInId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_TrasladoInventarioId",
                table: "Kardex",
                newName: "IX_Kardex_ProductInId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductIns_ProductInId",
                table: "Kardex",
                column: "ProductInId",
                principalTable: "ProductIns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductIns_ProductInId",
                table: "Kardex");

            migrationBuilder.RenameColumn(
                name: "ProductInId",
                table: "Kardex",
                newName: "TrasladoInventarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Kardex_ProductInId",
                table: "Kardex",
                newName: "IX_Kardex_TrasladoInventarioId");

            migrationBuilder.AddColumn<int>(
                name: "EntradaProductoId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleAnulationId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_EntradaProductoId",
                table: "Kardex",
                column: "EntradaProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_SaleAnulationId",
                table: "Kardex",
                column: "SaleAnulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_SaleId",
                table: "Kardex",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductIns_EntradaProductoId",
                table: "Kardex",
                column: "EntradaProductoId",
                principalTable: "ProductIns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductMovments_TrasladoInventarioId",
                table: "Kardex",
                column: "TrasladoInventarioId",
                principalTable: "ProductMovments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_SaleAnulations_SaleAnulationId",
                table: "Kardex",
                column: "SaleAnulationId",
                principalTable: "SaleAnulations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_Sales_SaleId",
                table: "Kardex",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
