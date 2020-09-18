using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class BattleMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedVideos_Battles_BattleId",
                table: "LikedVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedVideos_Semifinalists_SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_Semifinalists_Videos_VideoId",
                table: "Semifinalists");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Semifinalists_VideoId",
                table: "Semifinalists");

            migrationBuilder.DropIndex(
                name: "IX_LikedVideos_BattleId",
                table: "LikedVideos");

            migrationBuilder.DropIndex(
                name: "IX_LikedVideos_SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Semifinalists");

            migrationBuilder.DropColumn(
                name: "BattleId",
                table: "LikedVideos");

            migrationBuilder.DropColumn(
                name: "SemifinalistId",
                table: "LikedVideos");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Videos",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Semifinalists",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoName",
                table: "Semifinalists",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Battles",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Battles",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnersNumber",
                table: "Battles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BattleVote",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemifinalistId = table.Column<long>(nullable: false),
                    BattleId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleVote_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleVote_Semifinalists_SemifinalistId",
                        column: x => x.SemifinalistId,
                        principalTable: "Semifinalists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleVote_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleVote_BattleId",
                table: "BattleVote",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleVote_SemifinalistId",
                table: "BattleVote",
                column: "SemifinalistId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleVote_UserId",
                table: "BattleVote",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "BattleVote");

            migrationBuilder.DropColumn(
                name: "VideoName",
                table: "Semifinalists");

            migrationBuilder.DropColumn(
                name: "WinnersNumber",
                table: "Battles");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Videos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Semifinalists",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<long>(
                name: "VideoId",
                table: "Semifinalists",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BattleId",
                table: "LikedVideos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SemifinalistId",
                table: "LikedVideos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Battles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Battles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Accepted = table.Column<bool>(type: "bit", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromId = table.Column<long>(type: "bigint", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToId = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Semifinalists_VideoId",
                table: "Semifinalists",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_BattleId",
                table: "LikedVideos",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedVideos_SemifinalistId",
                table: "LikedVideos",
                column: "SemifinalistId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromId",
                table: "Messages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToId",
                table: "Messages",
                column: "ToId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Semifinalists_Videos_VideoId",
                table: "Semifinalists",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Users_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
