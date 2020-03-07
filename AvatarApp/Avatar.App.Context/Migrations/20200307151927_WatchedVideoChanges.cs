using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class WatchedVideoChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchedVideos_Users_UserId",
                table: "WatchedVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchedVideos_Videos_VideoId",
                table: "WatchedVideos");

            migrationBuilder.AlterColumn<long>(
                name: "VideoId",
                table: "WatchedVideos",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "WatchedVideos",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedVideos_Users_UserId",
                table: "WatchedVideos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedVideos_Videos_VideoId",
                table: "WatchedVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchedVideos_Users_UserId",
                table: "WatchedVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchedVideos_Videos_VideoId",
                table: "WatchedVideos");

            migrationBuilder.AlterColumn<long>(
                name: "VideoId",
                table: "WatchedVideos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "WatchedVideos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedVideos_Users_UserId",
                table: "WatchedVideos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedVideos_Videos_VideoId",
                table: "WatchedVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
