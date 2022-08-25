using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class MejorandoVentas2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CostoTotalDespuesDescuento",
                table: "SaleDetails",
                newName: "CostoTotalAntesDescuento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CostoTotalAntesDescuento",
                table: "SaleDetails",
                newName: "CostoTotalDespuesDescuento");
        }
    }
}
