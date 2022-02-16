using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class addProductInn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoAlmacens");

            migrationBuilder.CreateTable(
                name: "ProductIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoEntrada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    AlmacenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductIns_Almacen_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIns_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostoCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVentaMayor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVentaDetalle = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductInId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInDetails_ProductIns_ProductInId",
                        column: x => x.ProductInId,
                        principalTable: "ProductIns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInDetails_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInDetails_ProductInId",
                table: "ProductInDetails",
                column: "ProductInId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInDetails_ProductoId",
                table: "ProductInDetails",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIns_AlmacenId",
                table: "ProductIns",
                column: "AlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIns_ProviderId",
                table: "ProductIns",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductInDetails");

            migrationBuilder.DropTable(
                name: "ProductIns");

            migrationBuilder.CreateTable(
                name: "ProductoAlmacens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlmacenId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    RackId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoAlmacens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoAlmacens_Almacen_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoAlmacens_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoAlmacens_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAlmacens_AlmacenId",
                table: "ProductoAlmacens",
                column: "AlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAlmacens_ProductoId",
                table: "ProductoAlmacens",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAlmacens_RackId",
                table: "ProductoAlmacens",
                column: "RackId");
        }
    }
}
