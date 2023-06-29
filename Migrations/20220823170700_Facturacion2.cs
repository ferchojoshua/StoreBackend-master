using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Facturacion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaidById",
                table: "Facturacions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_PaidById",
                table: "Facturacions",
                column: "PaidById");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacions_AspNetUsers_PaidById",
                table: "Facturacions",
                column: "PaidById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturacions_AspNetUsers_PaidById",
                table: "Facturacions");

            migrationBuilder.DropIndex(
                name: "IX_Facturacions_PaidById",
                table: "Facturacions");

            migrationBuilder.DropColumn(
                name: "PaidById",
                table: "Facturacions");
        }
    }
}
