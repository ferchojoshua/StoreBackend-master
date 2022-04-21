using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipalities_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityId = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EditadoPor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEdicion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_AspNetUsers_CreadoPorId",
                        column: x => x.CreadoPorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CommunityId",
                table: "Clients",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CreadoPorId",
                table: "Clients",
                column: "CreadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_MunicipalityId",
                table: "Communities",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_DepartmentId",
                table: "Municipalities",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
