using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class productMovmentDetail2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlmacenDestinoId",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "AlmacenProcedenciaId",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "ProductMovmentDetails");

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "ProductMovments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlmacenDestinoId",
                table: "ProductMovmentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AlmacenProcedenciaId",
                table: "ProductMovmentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "AlmacenDestinoId",
                table: "ProductMovmentDetails");

            migrationBuilder.DropColumn(
                name: "AlmacenProcedenciaId",
                table: "ProductMovmentDetails");

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

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "ProductMovmentDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
