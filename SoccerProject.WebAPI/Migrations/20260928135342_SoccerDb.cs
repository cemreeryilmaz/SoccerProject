using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerProject.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SoccerDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    FixtureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamHalfScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamHalfScore = table.Column<int>(type: "int", nullable: false),
                    HomeTeamScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamScore = table.Column<int>(type: "int", nullable: false),
                    Stadium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    MatchStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.FixtureId);
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                    table.ForeignKey(
                        name: "FK_Fixtures_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "MatchEvents",
                columns: table => new
                {
                    MatchEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchEventType = table.Column<int>(type: "int", nullable: false),
                    Desctription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchEvents", x => x.MatchEventId);
                    table.ForeignKey(
                        name: "FK_MatchEvents_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "FixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchStatics",
                columns: table => new
                {
                    MatchStaticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    HomePossession = table.Column<int>(type: "int", nullable: false),
                    AwayPossession = table.Column<int>(type: "int", nullable: false),
                    HomeShoots = table.Column<int>(type: "int", nullable: false),
                    AwayShoots = table.Column<int>(type: "int", nullable: false),
                    HomeShootsOnTarget = table.Column<int>(type: "int", nullable: false),
                    AwayShootsOnTarget = table.Column<int>(type: "int", nullable: false),
                    HomePasses = table.Column<int>(type: "int", nullable: false),
                    AwayPasses = table.Column<int>(type: "int", nullable: false),
                    HomePassAccuracy = table.Column<int>(type: "int", nullable: false),
                    AwayPassAccuracy = table.Column<int>(type: "int", nullable: false),
                    HomeCorners = table.Column<int>(type: "int", nullable: false),
                    AwayCorners = table.Column<int>(type: "int", nullable: false),
                    HomeFouls = table.Column<int>(type: "int", nullable: false),
                    AwayFouls = table.Column<int>(type: "int", nullable: false),
                    HomeOffsides = table.Column<int>(type: "int", nullable: false),
                    AwayOffsides = table.Column<int>(type: "int", nullable: false),
                    HomeYellowCards = table.Column<int>(type: "int", nullable: false),
                    AwayYellowCards = table.Column<int>(type: "int", nullable: false),
                    HomeRedCards = table.Column<int>(type: "int", nullable: false),
                    AwayRedCards = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStatics", x => x.MatchStaticId);
                    table.ForeignKey(
                        name: "FK_MatchStatics_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "FixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_AwayTeamId",
                table: "Fixtures",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_HomeTeamId",
                table: "Fixtures",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_FixtureId",
                table: "MatchEvents",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStatics_FixtureId",
                table: "MatchStatics",
                column: "FixtureId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchEvents");

            migrationBuilder.DropTable(
                name: "MatchStatics");

            migrationBuilder.DropTable(
                name: "Fixtures");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
