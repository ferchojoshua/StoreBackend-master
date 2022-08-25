using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class MejorandoVentas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoXMonto",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DescuentoXPercent",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescuento",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoVentaTotal",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotalDespuesDescuento",
                table: "SaleDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DescuentoXPercent",
                table: "SaleDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescuento",
                table: "SaleDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescuentoXMonto",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DescuentoXPercent",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsDescuento",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "MontoVentaTotal",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CostoTotalDespuesDescuento",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "DescuentoXPercent",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "IsDescuento",
                table: "SaleDetails");
        }
    }
}
