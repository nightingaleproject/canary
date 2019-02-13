using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace canary.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    EndpointId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Finished = table.Column<bool>(nullable: false),
                    Record = table.Column<string>(nullable: true),
                    Issues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoints", x => x.EndpointId);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    RecordId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xml = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    Ije = table.Column<string>(nullable: true),
                    IjeInfo = table.Column<string>(nullable: true),
                    FhirInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.RecordId);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    CompletedDateTime = table.Column<DateTime>(nullable: false),
                    CompletedBool = table.Column<bool>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Correct = table.Column<int>(nullable: false),
                    Incorrect = table.Column<int>(nullable: false),
                    ReferenceRecord = table.Column<string>(nullable: true),
                    TestRecord = table.Column<string>(nullable: true),
                    Results = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Endpoints");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Tests");
        }
    }
}
