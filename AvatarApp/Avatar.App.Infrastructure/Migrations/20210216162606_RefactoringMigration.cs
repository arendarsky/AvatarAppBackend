using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class RefactoringMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalist",
                table: "Semifinalists");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "WatchedVideos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "WatchedVideos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "WatchedVideos");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "WatchedVideos");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalist",
                table: "Semifinalists",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
