using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddTipoPago2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPagoId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoPagoId",
                table: "Abonos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_TipoPagoId",
                table: "Sales",
                column: "TipoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Abonos_TipoPagoId",
                table: "Abonos",
                column: "TipoPagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abonos_TipoPagos_TipoPagoId",
                table: "Abonos",
                column: "TipoPagoId",
                principalTable: "TipoPagos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_TipoPagos_TipoPagoId",
                table: "Sales",
                column: "TipoPagoId",
                principalTable: "TipoPagos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abonos_TipoPagos_TipoPagoId",
                table: "Abonos");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_TipoPagos_TipoPagoId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_TipoPagoId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Abonos_TipoPagoId",
                table: "Abonos");

            migrationBuilder.DropColumn(
                name: "TipoPagoId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TipoPagoId",
                table: "Abonos");
        }
    }
}
