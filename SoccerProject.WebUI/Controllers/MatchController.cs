using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using SoccerProject.WebUI.DTOs.FixtureDtos;
using SoccerProject.WebUI.DTOs.TeamDtos;
using SoccerProject.WebUI.DTOs.StadiumDtos;

namespace SoccerProject.WebUI.Controllers
{
    public class MatchController : Controller
    {
        private const string FixturesApi = "https://localhost:7071/api/Fixtures";
        private const string TeamsApi = "https://localhost:7071/api/Teams";
        private const string StadiumsApi = "https://localhost:7071/api/Stadiums";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(FixturesApi);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<FixtureListDto>>(jsonData);
                return View(values);
            }

            return View(new List<FixtureListDto>());
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{FixturesApi}/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<GetByIdFixtureDto>(jsonData);
                if (value != null)
                    return View(value);
            }

            return Redirect("/Fixture");
        }

        [HttpGet]
        public async Task<IActionResult> CreateMatch()
        {
            await LoadTeamsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatch(CreateFixtureDto createFixtureDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(createFixtureDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(FixturesApi, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await LoadTeamsAsync();
            return View(createFixtureDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMatch(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{FixturesApi}/{id}");
            if (!responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetByIdFixtureDto>(jsonData);
            if (value == null)
                return RedirectToAction("Index");

            var updateDto = new UpdateFixtureDto
            {
                FixtureId = value.FixtureId,
                HomeTeamId = value.HomeTeamId,
                AwayTeamId = value.AwayTeamId,
                HomeTeamHalfScore = value.HomeTeamHalfScore,
                AwayTeamHalfScore = value.AwayTeamHalfScore,
                HomeTeamScore = value.HomeTeamScore,
                AwayTeamScore = value.AwayTeamScore,
                Stadium = value.Stadium,
                StadiumId = value.StadiumId,
                MatchDate = value.MatchDate,
                WeekNumber = value.WeekNumber,
                Status = value.Status
            };

            await LoadTeamsAsync();
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMatch(UpdateFixtureDto updateFixtureDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(updateFixtureDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync(FixturesApi, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            await LoadTeamsAsync();
            return View(updateFixtureDto);
        }

        public async Task<IActionResult> DeleteMatch(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.DeleteAsync($"{FixturesApi}/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        private async Task LoadTeamsAsync()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(TeamsApi);
            var teams = new List<ResultTeamDto>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                teams = JsonConvert.DeserializeObject<List<ResultTeamDto>>(jsonData) ?? new();
            }
            ViewBag.Teams = teams;

            // Stadyum dropdown'ı için
            var stadiumResponse = await client.GetAsync(StadiumsApi);
            var stadiums = new List<ResultStadiumDto>();
            if (stadiumResponse.IsSuccessStatusCode)
            {
                var stadiumJson = await stadiumResponse.Content.ReadAsStringAsync();
                stadiums = JsonConvert.DeserializeObject<List<ResultStadiumDto>>(stadiumJson) ?? new();
            }
            ViewBag.Stadiums = stadiums;
        }
    }
}
