using Microsoft.EntityFrameworkCore.Migrations;

namespace askLNU.DAL.Data.Migrations
{
    public partial class AddAnswerVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerVotes",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    AnswerId = table.Column<int>(nullable: false),
                    VotedUp = table.Column<bool>(nullable: false),
                    VotedDown = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerVotes", x => new { x.ApplicationUserId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_AnswerVotes_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerVotes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerVotes_AnswerId",
                table: "AnswerVotes",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerVotes");
        }
    }
}
