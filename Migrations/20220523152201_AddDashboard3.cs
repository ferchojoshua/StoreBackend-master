using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddDashboard3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContadorCompras",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TipoCliente",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_StoreId",
                table: "Sales",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Almacen_StoreId",
                table: "Sales",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Almacen_StoreId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_StoreId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ContadorCompras",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TipoCliente",
                table: "Clients");
        }
    }
}
