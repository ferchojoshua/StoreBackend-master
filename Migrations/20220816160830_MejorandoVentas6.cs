using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class MejorandoVentas6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontoVentaTotal",
                table: "Sales",
                newName: "MontoVentaAntesDescuento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontoVentaAntesDescuento",
                table: "Sales",
                newName: "MontoVentaTotal");
        }
    }
}
