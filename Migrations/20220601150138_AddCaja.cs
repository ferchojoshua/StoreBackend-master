using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddCaja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CajaTipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaTipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CajaMovments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CajaTipoId = table.Column<int>(type: "int", nullable: true),
                    Entradas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salidas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RealizadoPorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaMovments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CajaMovments_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaMovments_AspNetUsers_RealizadoPorId",
                        column: x => x.RealizadoPorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaMovments_CajaTipos_CajaTipoId",
                        column: x => x.CajaTipoId,
                        principalTable: "CajaTipos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CajaMovments_CajaTipoId",
                table: "CajaMovments",
                column: "CajaTipoId");

            migrationBuilder.CreateIndex(
                name: "IX_CajaMovments_RealizadoPorId",
                table: "CajaMovments",
                column: "RealizadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_CajaMovments_StoreId",
                table: "CajaMovments",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CajaMovments");

            migrationBuilder.DropTable(
                name: "CajaTipos");
        }
    }
}
