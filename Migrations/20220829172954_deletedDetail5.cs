using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class deletedDetail5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "SaleAnulations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleAnulations_StoreId",
                table: "SaleAnulations",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleAnulations_Almacen_StoreId",
                table: "SaleAnulations",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleAnulations_Almacen_StoreId",
                table: "SaleAnulations");

            migrationBuilder.DropIndex(
                name: "IX_SaleAnulations_StoreId",
                table: "SaleAnulations");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "SaleAnulations");
        }
    }
}
