namespace SoccerProject.WebUI.DTOs.StadiumDtos
{
    public class UpdateStadiumDto
    {
        public int StadiumId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
