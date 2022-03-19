using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesId = table.Column<int>(type: "int", nullable: true),
                    PermisosId = table.Column<int>(type: "int", nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolPermissions");
        }
    }
}
