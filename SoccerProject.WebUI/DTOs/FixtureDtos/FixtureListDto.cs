using SoccerProject.WebUI.DTOs.TeamDtos;
using SoccerProject.WebUI.Enums;

namespace SoccerProject.WebUI.DTOs.FixtureDtos
{
    public class FixtureListDto
    {
        public int FixtureId { get; set; }

        public int HomeTeamId { get; set; }
        public ResultTeamDto? HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public ResultTeamDto? AwayTeam { get; set; }

        public int HomeTeamHalfScore { get; set; }
        public int AwayTeamHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public string Stadium { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
        public int WeekNumber { get; set; }
        public MatchStatus MatchStatus { get; set; }
    }
}
