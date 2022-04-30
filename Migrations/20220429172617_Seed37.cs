using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class Seed37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleDetails_SaleDetailId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleDetailId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleDetailId",
                table: "Sales");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsContado",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SalesId",
                table: "SaleDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SalesId",
                table: "SaleDetails",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_Sales_SalesId",
                table: "SaleDetails",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_Sales_SalesId",
                table: "SaleDetails");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetails_SalesId",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsContado",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "SaleDetails");

            migrationBuilder.AddColumn<int>(
                name: "SaleDetailId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleDetailId",
                table: "Sales",
                column: "SaleDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleDetails_SaleDetailId",
                table: "Sales",
                column: "SaleDetailId",
                principalTable: "SaleDetails",
                principalColumn: "Id");
        }
    }
}
