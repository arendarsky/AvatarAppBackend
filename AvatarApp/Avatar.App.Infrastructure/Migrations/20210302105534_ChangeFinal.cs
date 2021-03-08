using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class ChangeFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVotingStarted",
                table: "Finals");

            migrationBuilder.AddColumn<DateTime>(
                name: "VotingEndTime",
                table: "Finals",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VotingStartTime",
                table: "Finals",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingEndTime",
                table: "Finals");

            migrationBuilder.DropColumn(
                name: "VotingStartTime",
                table: "Finals");

            migrationBuilder.AddColumn<bool>(
                name: "IsVotingStarted",
                table: "Finals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
