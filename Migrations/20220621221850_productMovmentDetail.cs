using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class productMovmentDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMovments_Productos_ProductoId",
                table: "ProductMovments");

            migrationBuilder.DropIndex(
                name: "IX_ProductMovments_ProductoId",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "ProductMovments");

            migrationBuilder.DropColumn(
                name: "ProductoId",
                table: "ProductMovments");

            migrationBuilder.CreateTable(
                name: "ProductMovmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductMovmentsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMovmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMovmentDetails_ProductMovments_ProductMovmentsId",
                        column: x => x.ProductMovmentsId,
                        principalTable: "ProductMovments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMovmentDetails_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMovmentDetails_ProductMovmentsId",
                table: "ProductMovmentDetails",
                column: "ProductMovmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMovmentDetails_ProductoId",
                table: "ProductMovmentDetails",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMovmentDetails");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "ProductMovments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "ProductMovments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoId",
                table: "ProductMovments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMovments_ProductoId",
                table: "ProductMovments",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMovments_Productos_ProductoId",
                table: "ProductMovments",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
