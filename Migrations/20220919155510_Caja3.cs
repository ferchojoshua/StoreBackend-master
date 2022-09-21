using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Caja3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovmentDetails_ProductMovments_ProductMovmentsId",
                table: "ProductMovmentDetails");

            migrationBuilder.RenameColumn(
                name: "ProductMovmentsId",
                table: "ProductMovmentDetails",
                newName: "ProductMovmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMovmentDetails_ProductMovmentsId",
                table: "ProductMovmentDetails",
                newName: "IX_ProductMovmentDetails_ProductMovmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovmentDetails_ProductMovments_ProductMovmentId",
                table: "ProductMovmentDetails",
                column: "ProductMovmentId",
                principalTable: "ProductMovments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovmentDetails_ProductMovments_ProductMovmentId",
                table: "ProductMovmentDetails");

            migrationBuilder.RenameColumn(
                name: "ProductMovmentId",
                table: "ProductMovmentDetails",
                newName: "ProductMovmentsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductMovmentDetails_ProductMovmentId",
                table: "ProductMovmentDetails",
                newName: "IX_ProductMovmentDetails_ProductMovmentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovmentDetails_ProductMovments_ProductMovmentsId",
                table: "ProductMovmentDetails",
                column: "ProductMovmentsId",
                principalTable: "ProductMovments",
                principalColumn: "Id");
        }
    }
}
