using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerProject.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerProfileCareerAndFixtureStadium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "MatchEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StadiumId",
                table: "Fixtures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlayerCareers",
                columns: table => new
                {
                    PlayerCareerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCareers", x => x.PlayerCareerId);
                    table.ForeignKey(
                        name: "FK_PlayerCareers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_PlayerId",
                table: "MatchEvents",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_StadiumId",
                table: "Fixtures",
                column: "StadiumId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCareers_PlayerId",
                table: "PlayerCareers",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Stadiums_StadiumId",
                table: "Fixtures",
                column: "StadiumId",
                principalTable: "Stadiums",
                principalColumn: "StadiumId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_Players_PlayerId",
                table: "MatchEvents",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Stadiums_StadiumId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Players_PlayerId",
                table: "MatchEvents");

            migrationBuilder.DropTable(
                name: "PlayerCareers");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_PlayerId",
                table: "MatchEvents");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_StadiumId",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "StadiumId",
                table: "Fixtures");
        }
    }
}
