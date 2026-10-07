namespace SoccerProject.WebUI.DTOs.PlayerCareerDtos
{
    public class CreatePlayerCareerDto
    {
        public int PlayerId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
