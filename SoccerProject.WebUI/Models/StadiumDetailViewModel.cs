using SoccerProject.WebUI.DTOs.FixtureDtos;
using SoccerProject.WebUI.DTOs.StadiumDtos;

namespace SoccerProject.WebUI.Models
{
    public class StadiumDetailViewModel
    {
        public ResultStadiumDto Stadium { get; set; } = new();
        public List<ResultFixtureDto> Fixtures { get; set; } = new();
    }
}
