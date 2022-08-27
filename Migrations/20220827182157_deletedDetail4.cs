using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class deletedDetail4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleDetailsAnulations");

            migrationBuilder.CreateTable(
                name: "SaleAnulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaAfectadaId = table.Column<int>(type: "int", nullable: true),
                    MontoAnulado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnulatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleAnulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleAnulations_AspNetUsers_AnulatedById",
                        column: x => x.AnulatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleAnulations_Sales_VentaAfectadaId",
                        column: x => x.VentaAfectadaId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleAnulationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CantidadAnulada = table.Column<int>(type: "int", nullable: false),
                    SaleDetailAfectadoId = table.Column<int>(type: "int", nullable: true),
                    SaleAnulationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleAnulationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleAnulationDetails_SaleAnulations_SaleAnulationId",
                        column: x => x.SaleAnulationId,
                        principalTable: "SaleAnulations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleAnulationDetails_SaleDetails_SaleDetailAfectadoId",
                        column: x => x.SaleDetailAfectadoId,
                        principalTable: "SaleDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleAnulationDetails_SaleAnulationId",
                table: "SaleAnulationDetails",
                column: "SaleAnulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleAnulationDetails_SaleDetailAfectadoId",
                table: "SaleAnulationDetails",
                column: "SaleDetailAfectadoId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleAnulations_AnulatedById",
                table: "SaleAnulations",
                column: "AnulatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SaleAnulations_VentaAfectadaId",
                table: "SaleAnulations",
                column: "VentaAfectadaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleAnulationDetails");

            migrationBuilder.DropTable(
                name: "SaleAnulations");

            migrationBuilder.CreateTable(
                name: "SaleDetailsAnulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnulatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SaleDetailId = table.Column<int>(type: "int", nullable: true),
                    CantidadAnulada = table.Column<int>(type: "int", nullable: false),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetailsAnulations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetailsAnulations_AspNetUsers_AnulatedById",
                        column: x => x.AnulatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleDetailsAnulations_SaleDetails_SaleDetailId",
                        column: x => x.SaleDetailId,
                        principalTable: "SaleDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetailsAnulations_AnulatedById",
                table: "SaleDetailsAnulations",
                column: "AnulatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetailsAnulations_SaleDetailId",
                table: "SaleDetailsAnulations",
                column: "SaleDetailId");
        }
    }
}
