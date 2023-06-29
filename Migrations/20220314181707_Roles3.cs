using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Roles3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserRol_RolId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserRols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRol",
                table: "UserRol");

            migrationBuilder.RenameTable(
                name: "UserRol",
                newName: "Rols");

            migrationBuilder.RenameIndex(
                name: "IX_UserRol_RoleName",
                table: "Rols",
                newName: "IX_Rols_RoleName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rols",
                table: "Rols",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rols_RolId",
                table: "AspNetUsers",
                column: "RolId",
                principalTable: "Rols",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rols_RolId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rols",
                table: "Rols");

            migrationBuilder.RenameTable(
                name: "Rols",
                newName: "UserRol");

            migrationBuilder.RenameIndex(
                name: "IX_Rols_RoleName",
                table: "UserRol",
                newName: "IX_UserRol_RoleName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRol",
                table: "UserRol",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserRols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisosId = table.Column<int>(type: "int", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRols_Permissions_PermisosId",
                        column: x => x.PermisosId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRols_UserRol_RolesId",
                        column: x => x.RolesId,
                        principalTable: "UserRol",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRols_PermisosId",
                table: "UserRols",
                column: "PermisosId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRols_RolesId",
                table: "UserRols",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserRol_RolId",
                table: "AspNetUsers",
                column: "RolId",
                principalTable: "UserRol",
                principalColumn: "Id");
        }
    }
}
