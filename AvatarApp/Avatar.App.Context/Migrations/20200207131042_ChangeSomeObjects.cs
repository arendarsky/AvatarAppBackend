using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class ChangeSomeObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WatchedUserId",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WatchedVideoId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_WatchedUserId",
                table: "Videos",
                column: "WatchedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WatchedVideoId",
                table: "Users",
                column: "WatchedVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Videos_WatchedVideoId",
                table: "Users",
                column: "WatchedVideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Users_WatchedUserId",
                table: "Videos",
                column: "WatchedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Videos_WatchedVideoId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Users_WatchedUserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_WatchedUserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Users_WatchedVideoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WatchedUserId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "WatchedVideoId",
                table: "Users");
        }
    }
}
