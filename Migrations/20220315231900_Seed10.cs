using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRol");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RolId",
                table: "Permissions",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Rols_RolId",
                table: "Permissions",
                column: "RolId",
                principalTable: "Rols",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Rols_RolId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_RolId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Permissions");

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
    }
}
