using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Facturacion1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoDescuento",
                table: "FacturaDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotalAntesDescuento",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoTotalDespuesDescuento",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoUnitario",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoXPercent",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescuento",
                table: "FacturaDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PVD",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PVM",
                table: "FacturaDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CodigoDescuento",
                table: "Facturacions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoXMonto",
                table: "Facturacions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoXPercent",
                table: "Facturacions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsContado",
                table: "Facturacions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescuento",
                table: "Facturacions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoVentaAntesDescuento",
                table: "Facturacions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                table: "Facturacions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoDescuento",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "CostoTotalAntesDescuento",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "CostoTotalDespuesDescuento",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "CostoUnitario",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "DescuentoXPercent",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "IsDescuento",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "PVD",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "PVM",
                table: "FacturaDetails");

            migrationBuilder.DropColumn(
                name: "CodigoDescuento",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "DescuentoXMonto",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "DescuentoXPercent",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "IsContado",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "IsDescuento",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "MontoVentaAntesDescuento",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                table: "Facturacions");
        }
    }
}
