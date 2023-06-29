using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddDashboard2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_StoreId",
                table: "Clients",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Almacen_StoreId",
                table: "Clients",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Almacen_StoreId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_StoreId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Clients");
        }
    }
}
