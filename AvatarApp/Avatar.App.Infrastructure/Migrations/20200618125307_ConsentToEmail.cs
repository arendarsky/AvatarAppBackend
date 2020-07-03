using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class ConsentToEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConsentToGeneralEmail",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsentToGeneralEmail",
                table: "Users");
        }
    }
}
