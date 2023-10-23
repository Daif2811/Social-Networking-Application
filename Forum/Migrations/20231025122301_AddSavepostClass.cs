using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.Migrations
{
    public partial class AddSavepostClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_ReplyToComments_PublishDate",
                table: "ReplyToComments");

            migrationBuilder.DropIndex(
                name: "IX_ReplyToComments_UserId_PublishDate",
                table: "ReplyToComments");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PublishDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId_PublishDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PublishDate",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId_PublishDate",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "SavePosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavePosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavePosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SavePosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavePosts_PostId",
                table: "SavePosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_SavePosts_UserId",
                table: "SavePosts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavePosts");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyToComments_PublishDate",
                table: "ReplyToComments",
                column: "PublishDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyToComments_UserId_PublishDate",
                table: "ReplyToComments",
                columns: new[] { "UserId", "PublishDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostId",
                table: "Posts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PublishDate",
                table: "Posts",
                column: "PublishDate");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId_PublishDate",
                table: "Posts",
                columns: new[] { "UserId", "PublishDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PublishDate",
                table: "Comments",
                column: "PublishDate");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId_PublishDate",
                table: "Comments",
                columns: new[] { "UserId", "PublishDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
