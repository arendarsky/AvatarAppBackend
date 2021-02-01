using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Infrastructure.Migrations
{
    public partial class FinalComponentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleVote_Battles_BattleId",
                table: "BattleVote");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleVote_Semifinalists_SemifinalistId",
                table: "BattleVote");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleVote_Users_UserId",
                table: "BattleVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BattleVote",
                table: "BattleVote");

            migrationBuilder.RenameTable(
                name: "BattleVote",
                newName: "BattleVotes");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVote_UserId",
                table: "BattleVotes",
                newName: "IX_BattleVotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVote_SemifinalistId",
                table: "BattleVotes",
                newName: "IX_BattleVotes_SemifinalistId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVote_BattleId",
                table: "BattleVotes",
                newName: "IX_BattleVotes_BattleId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalist",
                table: "Semifinalists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Battles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BattleVotes",
                table: "BattleVotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVotes_Battles_BattleId",
                table: "BattleVotes",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVotes_Semifinalists_SemifinalistId",
                table: "BattleVotes",
                column: "SemifinalistId",
                principalTable: "Semifinalists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVotes_Users_UserId",
                table: "BattleVotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleVotes_Battles_BattleId",
                table: "BattleVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleVotes_Semifinalists_SemifinalistId",
                table: "BattleVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleVotes_Users_UserId",
                table: "BattleVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BattleVotes",
                table: "BattleVotes");

            migrationBuilder.DropColumn(
                name: "IsFinalist",
                table: "Semifinalists");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Battles");

            migrationBuilder.RenameTable(
                name: "BattleVotes",
                newName: "BattleVote");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVotes_UserId",
                table: "BattleVote",
                newName: "IX_BattleVote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVotes_SemifinalistId",
                table: "BattleVote",
                newName: "IX_BattleVote_SemifinalistId");

            migrationBuilder.RenameIndex(
                name: "IX_BattleVotes_BattleId",
                table: "BattleVote",
                newName: "IX_BattleVote_BattleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BattleVote",
                table: "BattleVote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVote_Battles_BattleId",
                table: "BattleVote",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVote_Semifinalists_SemifinalistId",
                table: "BattleVote",
                column: "SemifinalistId",
                principalTable: "Semifinalists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleVote_Users_UserId",
                table: "BattleVote",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
