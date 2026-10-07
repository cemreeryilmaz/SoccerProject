using SoccerProject.WebAPI.DTOs.MatchEventDtos;
using SoccerProject.WebAPI.DTOs.MatchStaticDtos;
using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.DTOs.FixtureDtos
{
    public class GetByIdFixtureDto
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


        public string Stadium { get; set; }

        // Gerçek stadyum kaydına link için (varsa)
        public int? StadiumId { get; set; }

        public int WeekNumber { get; set; }

        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; }

        public MatchStaticDto MatchStatic { get; set; }
        public List<MatchEventDto> MatchEvents { get; set; }
    }
}
