using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddDashboard1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Abonos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abonos_StoreId",
                table: "Abonos",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abonos_Almacen_StoreId",
                table: "Abonos",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abonos_Almacen_StoreId",
                table: "Abonos");

            migrationBuilder.DropIndex(
                name: "IX_Abonos_StoreId",
                table: "Abonos");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Abonos");
        }
    }
}
