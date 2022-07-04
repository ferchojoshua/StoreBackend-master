using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Counts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    Concepto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entrada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counts_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Counts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Counts_Counts_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Counts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CountMovments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountGroupId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountMovments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountMovments_CountGroups_CountGroupId",
                        column: x => x.CountGroupId,
                        principalTable: "CountGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_CountGroupId",
                table: "CountMovments",
                column: "CountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Counts_CuentaId",
                table: "Counts",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Counts_StoreId",
                table: "Counts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Counts_UserId",
                table: "Counts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountMovments");

            migrationBuilder.DropTable(
                name: "Counts");

            migrationBuilder.DropTable(
                name: "CountGroups");
        }
    }
}
