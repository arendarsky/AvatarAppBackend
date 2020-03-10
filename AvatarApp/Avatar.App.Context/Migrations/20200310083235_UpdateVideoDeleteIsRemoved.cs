using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class UpdateVideoDeleteIsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Videos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
