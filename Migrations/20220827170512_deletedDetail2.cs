using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class deletedDetail2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleDetailsAnulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CantidadAnulada = table.Column<int>(type: "int", nullable: false),
                    SaleDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetailsAnulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetailsAnulations_SaleDetails_SaleDetailId",
                        column: x => x.SaleDetailId,
                        principalTable: "SaleDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetailsAnulations_SaleDetailId",
                table: "SaleDetailsAnulations",
                column: "SaleDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDetailsAnulations");
        }
    }
}
