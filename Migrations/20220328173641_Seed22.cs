using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserDevice",
                table: "UserSession",
                newName: "UserSO");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDateToken",
                table: "UserSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserBrowser",
                table: "UserSession",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDateToken",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "UserBrowser",
                table: "UserSession");

            migrationBuilder.RenameColumn(
                name: "UserSO",
                table: "UserSession",
                newName: "UserDevice");
        }
    }
}
