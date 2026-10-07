using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.DTOs.MatchEventDtos
{
    public class CreateMatchEventDto
    {
        public int FixtureId { get; set; }

        public int TeamId { get; set; }
        public string PlayerName { get; set; }

        public MatchEventType EventType { get; set; }
        public string? Description { get; set; }
        public int Minute { get; set; }
    }
}
