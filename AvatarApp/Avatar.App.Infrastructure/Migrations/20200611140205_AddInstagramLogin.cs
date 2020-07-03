using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class AddInstagramLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstagramLogin",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstagramLogin",
                table: "Users");
        }
    }
}
