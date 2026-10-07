using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.DTOs.FixtureDtos
{
    public class ResultFixtureDto
    {
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeLogoUrl { get; set; }
        public string AwayLogoUrl { get; set; }

        public int HomeTeamHalfScore { get; set; }
        public int AwayTeamHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public string[] HomeTeamForm { get; set; }
        public string[] AwayTeamForm { get; set; }

        public string Stadium { get; set; }
        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }
    }
}
