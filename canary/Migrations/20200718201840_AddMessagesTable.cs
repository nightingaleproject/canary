using Microsoft.EntityFrameworkCore.Migrations;

namespace canary.Migrations
{
    public partial class AddMessagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestMessageMessageId",
                table: "Tests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestMessageMessageId",
                table: "Tests",
                column: "TestMessageMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TestMessageMessageId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestMessageMessageId",
                table: "Tests");
        }
    }
}
