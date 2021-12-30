using Microsoft.EntityFrameworkCore.Migrations;

namespace WomenSafety.Migrations
{
    public partial class fix_point : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClusterGroup",
                table: "Points");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClusterGroup",
                table: "Points",
                type: "integer",
                nullable: true);
        }
    }
}
