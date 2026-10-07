using SoccerProject.WebUI.Enums;

namespace SoccerProject.WebUI.DTOs.MatchEventDtos
{
    public class CreateMatchEventDto
    {
        public int FixtureId { get; set; }
        public int TeamId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public MatchEventType EventType { get; set; }
        public string? Description { get; set; }
        public int Minute { get; set; }
    }
}
