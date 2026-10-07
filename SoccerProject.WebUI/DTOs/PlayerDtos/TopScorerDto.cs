namespace SoccerProject.WebUI.DTOs.PlayerDtos
{
    // Gol krallığı tablosu satır modeli
    public class TopScorerDto
    {
        public int Rank { get; set; }
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public string? Position { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Nationality { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Goals { get; set; }
    }
}
