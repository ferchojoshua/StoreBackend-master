using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlmacenDestinoId",
                table: "ProductMovments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AlmacenProcedenciaId",
                table: "ProductMovments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "ProductMovments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "ProductMovments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlmacenDestinoId",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "AlmacenProcedenciaId",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "ProductMovments");
        }
    }
}
