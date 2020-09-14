using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    FireBaseId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    ProfilePhoto = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InstagramLogin = table.Column<string>(nullable: true),
                    ConsentToGeneralEmail = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromId = table.Column<long>(nullable: true),
                    ToId = table.Column<long>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Accepted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ToId",
                        column: x => x.ToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    StartTime = table.Column<double>(nullable: false),
                    EndTime = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Semifinalists",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    VideoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semifinalists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Semifinalists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Semifinalists_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WatchedVideos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    VideoId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchedVideos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattleSemifinalists",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleId = table.Column<long>(nullable: false),
                    SemifinalistId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleSemifinalists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleSemifinalists_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleSemifinalists_Semifinalists_SemifinalistId",
                        column: x => x.SemifinalistId,
                        principalTable: "Semifinalists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikedVideos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    VideoId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SemifinalistId = table.Column<long>(nullable: true),
                    BattleId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikedVideos_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikedVideos_Semifinalists_SemifinalistId",
                        column: x => x.SemifinalistId,
                        principalTable: "Semifinalists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LikedVideos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleSemifinalists_BattleId",
                table: "BattleSemifinalists",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleSemifinalists_SemifinalistId",
                table: "BattleSemifinalists",
                column: "SemifinalistId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_BattleId",
                table: "LikedVideos",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_SemifinalistId",
                table: "LikedVideos",
                column: "SemifinalistId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_UserId",
                table: "LikedVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_VideoId",
                table: "LikedVideos",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromId",
                table: "Messages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToId",
                table: "Messages",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Semifinalists_UserId",
                table: "Semifinalists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Semifinalists_VideoId",
                table: "Semifinalists",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Guid",
                table: "Users",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_Name",
                table: "Videos",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedVideos_UserId",
                table: "WatchedVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedVideos_VideoId",
                table: "WatchedVideos",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleSemifinalists");

            migrationBuilder.DropTable(
                name: "LikedVideos");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "WatchedVideos");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Semifinalists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
