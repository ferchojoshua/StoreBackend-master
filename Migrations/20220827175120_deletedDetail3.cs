using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class deletedDetail3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnulatedById",
                table: "SaleDetailsAnulations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetailsAnulations_AnulatedById",
                table: "SaleDetailsAnulations",
                column: "AnulatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetailsAnulations_AspNetUsers_AnulatedById",
                table: "SaleDetailsAnulations",
                column: "AnulatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetailsAnulations_AspNetUsers_AnulatedById",
                table: "SaleDetailsAnulations");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetailsAnulations_AnulatedById",
                table: "SaleDetailsAnulations");

            migrationBuilder.DropColumn(
                name: "AnulatedById",
                table: "SaleDetailsAnulations");
        }
    }
}
