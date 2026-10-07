namespace SoccerProject.WebUI.DTOs.StadiumDtos
{
    public class CreateStadiumDto
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
