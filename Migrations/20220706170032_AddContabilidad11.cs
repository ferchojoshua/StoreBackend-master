using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FuentesContables",
                table: "FuentesContables");

            migrationBuilder.RenameTable(
                name: "FuentesContables",
                newName: "CountFuentesContables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountFuentesContables",
                table: "CountFuentesContables",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CountFuentesContables",
                table: "CountFuentesContables");

            migrationBuilder.RenameTable(
                name: "CountFuentesContables",
                newName: "FuentesContables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuentesContables",
                table: "FuentesContables",
                column: "Id");
        }
    }
}
