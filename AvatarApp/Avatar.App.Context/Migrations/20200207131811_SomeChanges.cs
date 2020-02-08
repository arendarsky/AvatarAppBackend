using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class SomeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "WatchedVideo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: true),
                    VideoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchedVideo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WatchedVideo_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchedVideo_UserId",
                table: "WatchedVideo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedVideo_VideoId",
                table: "WatchedVideo",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedVideo");

            migrationBuilder.AddColumn<long>(
                name: "WatchedUserId",
                table: "Videos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WatchedVideoId",
                table: "Users",
                type: "bigint",
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
    }
}
