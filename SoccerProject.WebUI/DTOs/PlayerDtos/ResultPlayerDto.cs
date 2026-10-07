namespace SoccerProject.WebUI.DTOs.PlayerDtos
{
    public class ResultPlayerDto
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int ShirtNumber { get; set; }
        public string Position { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public int? Age { get; set; }
    }
}
