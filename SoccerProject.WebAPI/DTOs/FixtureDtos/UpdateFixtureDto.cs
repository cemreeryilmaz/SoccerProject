using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.DTOs.FixtureDtos
{
    public class UpdateFixtureDto
    {
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public int HomeTeamHalfScore { get; set; }
        public int AwayTeamHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public string Stadium { get; set; }
        public int? StadiumId { get; set; }
        public DateTime MatchDate { get; set; }
        public int WeekNumber { get; set; }
        public MatchStatus Status { get; set; }
    }
}
