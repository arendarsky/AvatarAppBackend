using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class UserChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Users",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_Guid",
                table: "Users",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Guid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Users");
        }
    }
}
