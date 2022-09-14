using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Caja2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Facturacions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_SaleId",
                table: "Facturacions",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacions_Sales_SaleId",
                table: "Facturacions",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturacions_Sales_SaleId",
                table: "Facturacions");

            migrationBuilder.DropIndex(
                name: "IX_Facturacions_SaleId",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Facturacions");
        }
    }
}
