using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Avatar.App.Context.Migrations
{
    public partial class AddMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Videos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Videos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromId = table.Column<long>(nullable: true),
                    ToId = table.Column<long>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Confirmed = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Users_ToId",
                        column: x => x.ToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromId",
                table: "Message",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ToId",
                table: "Message",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Videos");
        }
    }
}
