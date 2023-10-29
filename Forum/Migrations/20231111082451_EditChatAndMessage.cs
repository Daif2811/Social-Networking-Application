using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Migrations
{
    public partial class EditChatAndMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Chats",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Chats",
                newName: "ChatId");
        }
    }
}
