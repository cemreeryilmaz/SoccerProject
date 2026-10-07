using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.DTOs.MatchEventDtos
{
    public class UpdateMatchEventDto
    {
        public int MatchEventId { get; set; }
        public int FixtureId { get; set; }

        public int TeamId { get; set; }
        public string PlayerName { get; set; }

        public MatchEventType EventType { get; set; }
        public string? Description { get; set; }
        public int Minute { get; set; }
    }
}
