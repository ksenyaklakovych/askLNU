using Microsoft.EntityFrameworkCore.Migrations;

namespace askLNU.DAL.Data.Migrations
{
    public partial class AddVotesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserVotedQuestions",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    VotedUp = table.Column<bool>(nullable: false),
                    VotedDown = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserVotedQuestions", x => new { x.ApplicationUserId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserVotedQuestions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ApplicationUserVotedQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserVotedQuestions_QuestionId",
                table: "ApplicationUserVotedQuestions",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserVotedQuestions");
        }
    }
}
