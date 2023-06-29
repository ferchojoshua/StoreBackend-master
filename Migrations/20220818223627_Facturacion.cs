using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Facturacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturacions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEventual = table.Column<bool>(type: "bit", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    MontoVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacturedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    IsAnulado = table.Column<bool>(type: "bit", nullable: false),
                    AnulatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturacions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturacions_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturacions_AspNetUsers_AnulatedById",
                        column: x => x.AnulatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturacions_AspNetUsers_FacturedById",
                        column: x => x.FacturedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturacions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FacturaDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAnulado = table.Column<bool>(type: "bit", nullable: false),
                    AnulatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacturacionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturaDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacturaDetails_Almacen_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FacturaDetails_AspNetUsers_AnulatedById",
                        column: x => x.AnulatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FacturaDetails_Facturacions_FacturacionId",
                        column: x => x.FacturacionId,
                        principalTable: "Facturacions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FacturaDetails_Productos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_AnulatedById",
                table: "Facturacions",
                column: "AnulatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_ClientId",
                table: "Facturacions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_FacturedById",
                table: "Facturacions",
                column: "FacturedById");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacions_StoreId",
                table: "Facturacions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaDetails_AnulatedById",
                table: "FacturaDetails",
                column: "AnulatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaDetails_FacturacionId",
                table: "FacturaDetails",
                column: "FacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaDetails_ProductId",
                table: "FacturaDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturaDetails_StoreId",
                table: "FacturaDetails",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacturaDetails");

            migrationBuilder.DropTable(
                name: "Facturacions");
        }
    }
}
