namespace SoccerProject.WebUI.Models
{
    public class StandingsViewModel
    {
        public string Season { get; set; } = "2026/27";
        public int Week { get; set; }
        public int TeamCount { get; set; }
        public List<StandingRowViewModel> Rows { get; set; } = new();
    }
}
