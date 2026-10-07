using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerProject.WebUI.DTOs.FixtureDtos;
using SoccerProject.WebUI.Models;

namespace SoccerProject.WebUI.Controllers
{
    public class FixtureController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Fixtures";

        public async Task<IActionResult> Index(int? week)
        {
            var client = new HttpClient();

            var allResponse = await client.GetAsync(ApiBase);
            var availableWeeks = new List<int>();
            var weekRanges = new Dictionary<int, WeekRange>();
            if (allResponse.IsSuccessStatusCode)
            {
                var allJson = await allResponse.Content.ReadAsStringAsync();
                var all = JsonConvert.DeserializeObject<List<FixtureListDto>>(allJson) ?? new();
                availableWeeks = all.Select(x => x.WeekNumber).Distinct().OrderBy(x => x).ToList();
                weekRanges = all
                    .GroupBy(x => x.WeekNumber)
                    .ToDictionary(
                        g => g.Key,
                        g => new WeekRange { Start = g.Min(x => x.MatchDate), End = g.Max(x => x.MatchDate) });
            }

            int currentWeek = week ?? (availableWeeks.Contains(12) ? 12 : availableWeeks.FirstOrDefault());

            var model = new FixtureWeekViewModel
            {
                CurrentWeek = currentWeek,
                AvailableWeeks = availableWeeks,
                WeekRanges = weekRanges
            };

            if (currentWeek > 0)
            {
                var responseMessage = await client.GetAsync($"{ApiBase}/GetFixturesByWeek/{currentWeek}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    model.Fixtures = JsonConvert.DeserializeObject<List<ResultFixtureDto>>(jsonData) ?? new();
                }
            }

            return View(model);
        }
    }
}
