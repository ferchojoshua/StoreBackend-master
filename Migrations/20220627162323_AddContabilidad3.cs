using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counts_Counts_CuentaId",
                table: "Counts");

            migrationBuilder.RenameColumn(
                name: "CuentaId",
                table: "Counts",
                newName: "CountMovmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Counts_CuentaId",
                table: "Counts",
                newName: "IX_Counts_CountMovmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_CountMovments_CountMovmentId",
                table: "Counts",
                column: "CountMovmentId",
                principalTable: "CountMovments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counts_CountMovments_CountMovmentId",
                table: "Counts");

            migrationBuilder.RenameColumn(
                name: "CountMovmentId",
                table: "Counts",
                newName: "CuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_Counts_CountMovmentId",
                table: "Counts",
                newName: "IX_Counts_CuentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_Counts_CuentaId",
                table: "Counts",
                column: "CuentaId",
                principalTable: "Counts",
                principalColumn: "Id");
        }
    }
}
