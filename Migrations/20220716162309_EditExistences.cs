using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class EditExistences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioCompra",
                table: "Existences",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CuentasXCobrarDailyChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlmacenId = table.Column<int>(type: "int", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    MontoVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasXCobrarDailyChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasXCobrarDailyChecks_Almacen_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CuentasXCobrarDailyChecks_Clients_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentasXCobrarDailyChecks_AlmacenId",
                table: "CuentasXCobrarDailyChecks",
                column: "AlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasXCobrarDailyChecks_ClienteId",
                table: "CuentasXCobrarDailyChecks",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuentasXCobrarDailyChecks");

            migrationBuilder.DropColumn(
                name: "PrecioCompra",
                table: "Existences");
        }
    }
}
