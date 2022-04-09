using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Existencia",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioVentaDetalle",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioVentaMayor",
                table: "Productos");

            migrationBuilder.CreateTable(
                name: "Existences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlmacenId = table.Column<int>(type: "int", nullable: true),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Existencia = table.Column<int>(type: "int", nullable: false),
                    PrecioVentaMayor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioVentaDetalle = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Existences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Existences_Almacen_AlmacenId",
                        column: x => x.AlmacenId,
                        principalTable: "Almacen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Existences_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Existences_AlmacenId",
                table: "Existences",
                column: "AlmacenId");

            migrationBuilder.CreateIndex(
                name: "IX_Existences_ProductoId",
                table: "Existences",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Existences");

            migrationBuilder.AddColumn<int>(
                name: "Existencia",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioVentaDetalle",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioVentaMayor",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
