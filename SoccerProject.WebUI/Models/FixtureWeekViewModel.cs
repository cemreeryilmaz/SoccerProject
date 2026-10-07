using SoccerProject.WebUI.DTOs.FixtureDtos;

namespace SoccerProject.WebUI.Models
{
    public class FixtureWeekViewModel
    {
        public string Season { get; set; } = "2026/27";
        public int CurrentWeek { get; set; }
        public List<int> AvailableWeeks { get; set; } = new();
        public List<ResultFixtureDto> Fixtures { get; set; } = new();

        // Her haftanın tarih aralığı (ilk maç - son maç). Hafta şeridi ve dropdown için.
        public Dictionary<int, WeekRange> WeekRanges { get; set; } = new();
    }

    public class WeekRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
