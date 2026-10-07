namespace SoccerProject.WebUI.DTOs.MatchStaticDtos
{
    public class MatchStaticDto
    {
        public int MatchStaticId { get; set; }
        public int FixtureId { get; set; }

        public int HomePossession { get; set; }
        public int AwayPossession { get; set; }

        public int HomeShoots { get; set; }
        public int AwayShoots { get; set; }

        public int HomeShootsOnTarget { get; set; }
        public int AwayShootsOnTarget { get; set; }

        public int HomePasses { get; set; }
        public int AwayPasses { get; set; }

        public int HomePassAccuracy { get; set; }
        public int AwayPassAccuracy { get; set; }

        public int HomeCorners { get; set; }
        public int AwayCorners { get; set; }

        public int HomeFouls { get; set; }
        public int AwayFouls { get; set; }

        public int HomeOffsides { get; set; }
        public int AwayOffsides { get; set; }

        public int HomeYellowCards { get; set; }
        public int AwayYellowCards { get; set; }

        public int HomeRedCards { get; set; }
        public int AwayRedCards { get; set; }
    }
}
