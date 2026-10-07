namespace SoccerProject.WebUI.DTOs.FixtureDtos
{
    public class FixtureDropdownDto
    {
        public int FixtureId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public int WeekNumber { get; set; }
    }
}
