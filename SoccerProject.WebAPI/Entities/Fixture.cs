using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.Entities
{
    public class Fixture
    {
        public int FixtureId { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public int HomeTeamHalfScore { get; set; }
        public int AwayTeamHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public string Stadium { get; set; }

        // Stadyum artık gerçek bir Stadium kaydına da bağlanabilir (opsiyonel FK).
        public int? StadiumId { get; set; }
        public Stadium StadiumRef { get; set; }

        public DateTime MatchDate { get; set; }
        public int WeekNumber { get; set; }
        public MatchStatus MatchStatus { get; set; }

        public IList<MatchEvent> MatchEvents { get; set; }
        public MatchStatic MatchStatic { get; set; }

    }
}
