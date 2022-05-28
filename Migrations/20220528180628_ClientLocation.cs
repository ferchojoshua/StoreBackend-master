using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Store.Migrations
{
    public partial class ClientLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "ClientCoords",
                table: "Clients",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreComercial",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientCoords",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "NombreComercial",
                table: "Clients");
        }
    }
}
