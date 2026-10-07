namespace SoccerProject.WebUI.DTOs.PlayerCareerDtos
{
    public class ResultPlayerCareerDto
    {
        public int PlayerCareerId { get; set; }
        public int PlayerId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
