namespace SoccerProject.WebAPI.DTOs.PlayerCareerDtos
{
    public class CreatePlayerCareerDto
    {
        public int PlayerId { get; set; }
        public string ClubName { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
