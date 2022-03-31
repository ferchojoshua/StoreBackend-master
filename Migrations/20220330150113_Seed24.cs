using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditBy",
                table: "ProductIns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "ProductIns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIns_AspNetUsers_UsuarioId",
                table: "ProductIns");

            migrationBuilder.DropIndex(
                name: "IX_ProductIns_UsuarioId",
                table: "ProductIns");

            migrationBuilder.DropColumn(
                name: "EditBy",
                table: "ProductIns");

            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "ProductIns");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "ProductIns");
        }
    }
}
