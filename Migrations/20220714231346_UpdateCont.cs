using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Migrations
{
    public partial class UpdateCont : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Counts");
        }
    }
}
