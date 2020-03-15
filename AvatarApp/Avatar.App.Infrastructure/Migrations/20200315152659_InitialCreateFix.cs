using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class InitialCreateFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "test",
                table: "Users");
        }
    }
}
