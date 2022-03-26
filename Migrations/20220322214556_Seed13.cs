using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoNegocioId",
                table: "Familias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familias_TipoNegocioId",
                table: "Familias",
                column: "TipoNegocioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Familias_TipoNegocios_TipoNegocioId",
                table: "Familias",
                column: "TipoNegocioId",
                principalTable: "TipoNegocios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familias_TipoNegocios_TipoNegocioId",
                table: "Familias");

            migrationBuilder.DropIndex(
                name: "IX_Familias_TipoNegocioId",
                table: "Familias");

            migrationBuilder.DropColumn(
                name: "TipoNegocioId",
                table: "Familias");
        }
    }
}
