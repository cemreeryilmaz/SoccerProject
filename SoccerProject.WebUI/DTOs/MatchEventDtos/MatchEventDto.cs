using SoccerProject.WebUI.Enums;

namespace SoccerProject.WebUI.DTOs.MatchEventDtos
{
    public class MatchEventDto
    {
        public int TeamId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int? PlayerId { get; set; }   // Oyuncu profiline link için
        public MatchEventType EventType { get; set; }
        public string? Description { get; set; }
        public int Minute { get; set; }
    }
}
