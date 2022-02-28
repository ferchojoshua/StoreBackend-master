using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class reparaProductInn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInDetails_Productos_ProductoId",
                table: "ProductInDetails");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "ProductInDetails",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Costo",
                table: "ProductInDetails",
                newName: "CostoUnitario");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInDetails_ProductoId",
                table: "ProductInDetails",
                newName: "IX_ProductInDetails_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "TipoPago",
                table: "ProductIns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInDetails_Productos_ProductId",
                table: "ProductInDetails",
                column: "ProductId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInDetails_Productos_ProductId",
                table: "ProductInDetails");

            migrationBuilder.DropColumn(
                name: "TipoPago",
                table: "ProductIns");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductInDetails",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "CostoUnitario",
                table: "ProductInDetails",
                newName: "Costo");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInDetails_ProductId",
                table: "ProductInDetails",
                newName: "IX_ProductInDetails_ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInDetails_Productos_ProductoId",
                table: "ProductInDetails",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
