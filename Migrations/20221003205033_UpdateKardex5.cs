using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class UpdateKardex5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductMovmentsId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleAnulationId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesId",
                table: "Kardex",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_ProductMovmentsId",
                table: "Kardex",
                column: "ProductMovmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_SaleAnulationId",
                table: "Kardex",
                column: "SaleAnulationId");

            migrationBuilder.CreateIndex(
                name: "IX_Kardex_SalesId",
                table: "Kardex",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_ProductMovments_ProductMovmentsId",
                table: "Kardex",
                column: "ProductMovmentsId",
                principalTable: "ProductMovments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_SaleAnulations_SaleAnulationId",
                table: "Kardex",
                column: "SaleAnulationId",
                principalTable: "SaleAnulations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kardex_Sales_SalesId",
                table: "Kardex",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_ProductMovments_ProductMovmentsId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_SaleAnulations_SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropForeignKey(
                name: "FK_Kardex_Sales_SalesId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_ProductMovmentsId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropIndex(
                name: "IX_Kardex_SalesId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "ProductMovmentsId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "SaleAnulationId",
                table: "Kardex");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "Kardex");
        }
    }
}
