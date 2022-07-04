using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class AddContabilidad5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountMovments_CountGroups_CountGroupId",
                table: "CountMovments");

            migrationBuilder.DropForeignKey(
                name: "FK_Counts_Almacen_StoreId",
                table: "Counts");

            migrationBuilder.DropForeignKey(
                name: "FK_Counts_AspNetUsers_UserId",
                table: "Counts");

            migrationBuilder.DropForeignKey(
                name: "FK_Counts_CountMovments_CountMovmentId",
                table: "Counts");

            migrationBuilder.DropIndex(
                name: "IX_Counts_CountMovmentId",
                table: "Counts");

            migrationBuilder.DropIndex(
                name: "IX_Counts_UserId",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "CountMovmentId",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "Salida",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CountMovments");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "Counts",
                newName: "CountGroupId");

            migrationBuilder.RenameColumn(
                name: "Concepto",
                table: "Counts",
                newName: "Descripcion");

            migrationBuilder.RenameIndex(
                name: "IX_Counts_StoreId",
                table: "Counts",
                newName: "IX_Counts_CountGroupId");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "CountMovments",
                newName: "Concepto");

            migrationBuilder.RenameColumn(
                name: "CountGroupId",
                table: "CountMovments",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_CountMovments_CountGroupId",
                table: "CountMovments",
                newName: "IX_CountMovments_StoreId");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "Counts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountId",
                table: "CountMovments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Entrada",
                table: "CountMovments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "CountMovments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "CountMovments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Salida",
                table: "CountMovments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CountMovments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_CountId",
                table: "CountMovments",
                column: "CountId");

            migrationBuilder.CreateIndex(
                name: "IX_CountMovments_UserId",
                table: "CountMovments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountMovments_Almacen_StoreId",
                table: "CountMovments",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CountMovments_AspNetUsers_UserId",
                table: "CountMovments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CountMovments_Counts_CountId",
                table: "CountMovments",
                column: "CountId",
                principalTable: "Counts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_CountGroups_CountGroupId",
                table: "Counts",
                column: "CountGroupId",
                principalTable: "CountGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountMovments_Almacen_StoreId",
                table: "CountMovments");

            migrationBuilder.DropForeignKey(
                name: "FK_CountMovments_AspNetUsers_UserId",
                table: "CountMovments");

            migrationBuilder.DropForeignKey(
                name: "FK_CountMovments_Counts_CountId",
                table: "CountMovments");

            migrationBuilder.DropForeignKey(
                name: "FK_Counts_CountGroups_CountGroupId",
                table: "Counts");

            migrationBuilder.DropIndex(
                name: "IX_CountMovments_CountId",
                table: "CountMovments");

            migrationBuilder.DropIndex(
                name: "IX_CountMovments_UserId",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Counts");

            migrationBuilder.DropColumn(
                name: "CountId",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "Salida",
                table: "CountMovments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CountMovments");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Counts",
                newName: "Concepto");

            migrationBuilder.RenameColumn(
                name: "CountGroupId",
                table: "Counts",
                newName: "StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_Counts_CountGroupId",
                table: "Counts",
                newName: "IX_Counts_StoreId");

            migrationBuilder.RenameColumn(
                name: "StoreId",
                table: "CountMovments",
                newName: "CountGroupId");

            migrationBuilder.RenameColumn(
                name: "Concepto",
                table: "CountMovments",
                newName: "Descripcion");

            migrationBuilder.RenameIndex(
                name: "IX_CountMovments_StoreId",
                table: "CountMovments",
                newName: "IX_CountMovments_CountGroupId");

            migrationBuilder.AddColumn<int>(
                name: "CountMovmentId",
                table: "Counts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Entrada",
                table: "Counts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "Counts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Counts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Salida",
                table: "Counts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Counts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "CountMovments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Counts_CountMovmentId",
                table: "Counts",
                column: "CountMovmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Counts_UserId",
                table: "Counts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountMovments_CountGroups_CountGroupId",
                table: "CountMovments",
                column: "CountGroupId",
                principalTable: "CountGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_Almacen_StoreId",
                table: "Counts",
                column: "StoreId",
                principalTable: "Almacen",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_AspNetUsers_UserId",
                table: "Counts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Counts_CountMovments_CountMovmentId",
                table: "Counts",
                column: "CountMovmentId",
                principalTable: "CountMovments",
                principalColumn: "Id");
        }
    }
}
