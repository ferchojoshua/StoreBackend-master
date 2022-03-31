using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIns_AspNetUsers_UsuarioId",
                table: "ProductIns");

            migrationBuilder.DropIndex(
                name: "IX_ProductIns_UsuarioId",
                table: "ProductIns");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "ProductIns");

            migrationBuilder.AlterColumn<string>(
                name: "EditBy",
                table: "ProductIns",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductIns",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductIns");

            migrationBuilder.AlterColumn<int>(
                name: "EditBy",
                table: "ProductIns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "ProductIns",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductIns_UsuarioId",
                table: "ProductIns",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIns_AspNetUsers_UsuarioId",
                table: "ProductIns",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
