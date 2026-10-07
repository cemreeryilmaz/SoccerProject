using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerProject.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desctription",
                table: "MatchEvents",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MatchEvents",
                newName: "Desctription");
        }
    }
}
