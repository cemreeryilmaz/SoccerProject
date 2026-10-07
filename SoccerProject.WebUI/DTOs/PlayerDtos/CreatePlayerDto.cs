namespace SoccerProject.WebUI.DTOs.PlayerDtos
{
    public class CreatePlayerDto
    {
        public string FullName { get; set; } = string.Empty;
        public int ShirtNumber { get; set; }
        public string Position { get; set; } = string.Empty;
        public int TeamId { get; set; }

        public string? PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }
    }
}
