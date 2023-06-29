using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolPermissions");

            migrationBuilder.CreateTable(
                name: "PermissionRol",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRol", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PermissionRol_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRol_Rols_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRol_RolesId",
                table: "PermissionRol",
                column: "RolesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRol");

            migrationBuilder.CreateTable(
                name: "RolPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisosId = table.Column<int>(type: "int", nullable: true),
                    RolesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolPermissions_Permissions_PermisosId",
                        column: x => x.PermisosId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolPermissions_Rols_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Rols",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolPermissions_PermisosId",
                table: "RolPermissions",
                column: "PermisosId");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermissions_RolesId",
                table: "RolPermissions",
                column: "RolesId");
        }
    }
}
