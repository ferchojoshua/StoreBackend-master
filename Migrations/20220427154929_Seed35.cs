using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Almacen_AspNetUsers_UserId",
                table: "Almacen");

            migrationBuilder.DropIndex(
                name: "IX_Almacen_UserId",
                table: "Almacen");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Almacen");

            migrationBuilder.CreateTable(
                name: "AlmacenUser",
                columns: table => new
                {
                    StoreAccessId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlmacenUser", x => new { x.StoreAccessId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AlmacenUser_Almacen_StoreAccessId",
                        column: x => x.StoreAccessId,
                        principalTable: "Almacen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlmacenUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlmacenUser_UsersId",
                table: "AlmacenUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlmacenUser");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Almacen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Almacen_UserId",
                table: "Almacen",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Almacen_AspNetUsers_UserId",
                table: "Almacen",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
