using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class uodateINDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInDetails_ProductIns_ProductInId",
                table: "ProductInDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoFactura",
                table: "ProductIns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "ProductInId",
                table: "ProductInDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Costo",
                table: "ProductInDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInDetails_ProductIns_ProductInId",
                table: "ProductInDetails",
                column: "ProductInId",
                principalTable: "ProductIns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInDetails_ProductIns_ProductInId",
                table: "ProductInDetails");

            migrationBuilder.DropColumn(
                name: "MontoFactura",
                table: "ProductIns");

            migrationBuilder.DropColumn(
                name: "Costo",
                table: "ProductInDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ProductInId",
                table: "ProductInDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInDetails_ProductIns_ProductInId",
                table: "ProductInDetails",
                column: "ProductInId",
                principalTable: "ProductIns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
