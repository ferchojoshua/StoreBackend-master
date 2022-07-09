using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountAsientoContableId",
                table: "CountAsientoContableDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientoContableDetails_CountAsientoContableId",
                table: "CountAsientoContableDetails",
                column: "CountAsientoContableId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountAsientoContableDetails_CountAsientosContables_CountAsientoContableId",
                table: "CountAsientoContableDetails",
                column: "CountAsientoContableId",
                principalTable: "CountAsientosContables",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountAsientoContableDetails_CountAsientosContables_CountAsientoContableId",
                table: "CountAsientoContableDetails");

            migrationBuilder.DropIndex(
                name: "IX_CountAsientoContableDetails_CountAsientoContableId",
                table: "CountAsientoContableDetails");

            migrationBuilder.DropColumn(
                name: "CountAsientoContableId",
                table: "CountAsientoContableDetails");
        }
    }
}
