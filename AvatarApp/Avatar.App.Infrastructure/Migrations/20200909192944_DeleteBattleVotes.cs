using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class DeleteBattleVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleVotes");

            migrationBuilder.AddColumn<long>(
                name: "BattleId",
                table: "LikedVideos",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SemifinalistId",
                table: "LikedVideos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_BattleId",
                table: "LikedVideos",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_SemifinalistId",
                table: "LikedVideos",
                column: "SemifinalistId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedVideos_Battles_BattleId",
                table: "LikedVideos",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedVideos_Semifinalists_SemifinalistId",
                table: "LikedVideos",
                column: "SemifinalistId",
                principalTable: "Semifinalists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedVideos_Battles_BattleId",
                table: "LikedVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedVideos_Semifinalists_SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.DropIndex(
                name: "IX_LikedVideos_BattleId",
                table: "LikedVideos");

            migrationBuilder.DropIndex(
                name: "IX_LikedVideos_SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "LikedVideos");

            migrationBuilder.DropColumn(
                name: "SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.CreateTable(
                name: "BattleVotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BattleId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SemifinalistId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleVotes_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleVotes_Semifinalists_SemifinalistId",
                        column: x => x.SemifinalistId,
                        principalTable: "Semifinalists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleVotes_BattleId",
                table: "BattleVotes",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleVotes_SemifinalistId",
                table: "BattleVotes",
                column: "SemifinalistId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleVotes_UserId",
                table: "BattleVotes",
                column: "UserId");
        }
    }
}
