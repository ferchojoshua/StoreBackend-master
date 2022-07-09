using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountMovments");

            migrationBuilder.CreateTable(
                name: "CountAsientoContableDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuentaId = table.Column<int>(type: "int", nullable: true),
                    Debito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountAsientoContableDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountAsientoContableDetails_Counts_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Counts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CountAsientosContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LibroContableId = table.Column<int>(type: "int", nullable: true),
                    FuenteContableId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountAsientosContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountAsientosContables_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CountAsientosContables_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CountAsientosContables_CountFuentesContables_FuenteContableId",
                        column: x => x.FuenteContableId,
                        principalTable: "CountFuentesContables",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CountAsientosContables_CountLibros_LibroContableId",
                        column: x => x.LibroContableId,
                        principalTable: "CountLibros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientoContableDetails_CuentaId",
                table: "CountAsientoContableDetails",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientosContables_FuenteContableId",
                table: "CountAsientosContables",
                column: "FuenteContableId");

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientosContables_LibroContableId",
                table: "CountAsientosContables",
                column: "LibroContableId");

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientosContables_StoreId",
                table: "CountAsientosContables",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CountAsientosContables_UserId",
                table: "CountAsientosContables",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountAsientoContableDetails");

            migrationBuilder.DropTable(
                name: "CountAsientosContables");

            migrationBuilder.CreateTable(
                name: "CountMovments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Concepto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entrada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salida = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountMovments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountMovments_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CountMovments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CountMovments_Counts_CountId",
                        column: x => x.CountId,
                        principalTable: "Counts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_CountId",
                table: "CountMovments",
                column: "CountId");

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_StoreId",
                table: "CountMovments",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_UserId",
                table: "CountMovments",
                column: "UserId");
        }
    }
}
