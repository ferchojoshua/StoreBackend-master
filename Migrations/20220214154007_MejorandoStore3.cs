using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class MejorandoStore3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Almacen_AlmacenId",
                table: "Racks");

            migrationBuilder.AlterColumn<int>(
                name: "AlmacenId",
                table: "Racks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Almacen_AlmacenId",
                table: "Racks",
                column: "AlmacenId",
                principalTable: "Almacen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Racks_Almacen_AlmacenId",
                table: "Racks");

            migrationBuilder.AlterColumn<int>(
                name: "AlmacenId",
                table: "Racks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Racks_Almacen_AlmacenId",
                table: "Racks",
                column: "AlmacenId",
                principalTable: "Almacen",
                principalColumn: "Id");
        }
    }
}
