using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class CountCodeStructure2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClasificacionId",
                table: "Counts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Counts_ClasificacionId",
                table: "Counts",
                column: "ClasificacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_CountCodeStructures_ClasificacionId",
                table: "Counts",
                column: "ClasificacionId",
                principalTable: "CountCodeStructures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counts_CountCodeStructures_ClasificacionId",
                table: "Counts");

            migrationBuilder.DropIndex(
                name: "IX_Counts_ClasificacionId",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "ClasificacionId",
                table: "Counts");
        }
    }
}
