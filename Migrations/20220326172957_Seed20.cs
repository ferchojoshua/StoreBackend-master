using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSessionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserDevice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserSessionId",
                table: "AspNetUsers",
                column: "UserSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserSession_UserSessionId",
                table: "AspNetUsers",
                column: "UserSessionId",
                principalTable: "UserSession",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserSession_UserSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSessionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSessionId",
                table: "AspNetUsers");
        }
    }
}
