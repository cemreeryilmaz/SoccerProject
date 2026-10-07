using SoccerProject.WebUI.Enums;

namespace SoccerProject.WebUI.DTOs.FixtureDtos
{
    public class ResultFixtureDto
    {
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public string HomeLogoUrl { get; set; } = string.Empty;
        public string AwayLogoUrl { get; set; } = string.Empty;

        public int HomeTeamHalfScore { get; set; }
        public int AwayTeamHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public string[] HomeTeamForm { get; set; } = Array.Empty<string>();
        public string[] AwayTeamForm { get; set; } = Array.Empty<string>();

        public string Stadium { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
    }
}
