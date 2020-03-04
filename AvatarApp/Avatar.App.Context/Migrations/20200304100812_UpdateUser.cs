using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_FromId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_ToId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ToId",
                table: "Messages",
                newName: "IX_Messages_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_FromId",
                table: "Messages",
                newName: "IX_Messages_FromId");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_FromId",
                table: "Messages",
                column: "FromId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ToId",
                table: "Messages",
                column: "ToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_FromId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ToId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ToId",
                table: "Message",
                newName: "IX_Message_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FromId",
                table: "Message",
                newName: "IX_Message_FromId");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Message",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_FromId",
                table: "Message",
                column: "FromId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_ToId",
                table: "Message",
                column: "ToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
