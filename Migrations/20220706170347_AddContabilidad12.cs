using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Condigo",
                table: "CountLibros",
                newName: "Codigo");

            migrationBuilder.RenameColumn(
                name: "Condigo",
                table: "CountFuentesContables",
                newName: "Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "CountLibros",
                newName: "Condigo");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "CountFuentesContables",
                newName: "Condigo");
        }
    }
}
