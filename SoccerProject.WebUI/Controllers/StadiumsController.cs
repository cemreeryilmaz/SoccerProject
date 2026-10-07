using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerProject.WebUI.DTOs.FixtureDtos;
using SoccerProject.WebUI.DTOs.StadiumDtos;
using SoccerProject.WebUI.Models;

namespace SoccerProject.WebUI.Controllers
{
    // Herkese açık (ön yüz) Stadyum sayfaları - /Stadiums
    public class StadiumsController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Stadiums";
        private const string FixturesApi = "https://localhost:7071/api/Fixtures";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(ApiBase);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultStadiumDto>>(jsonData) ?? new();
                return View(values);
            }
            return View(new List<ResultStadiumDto>());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetStadiumById?id={id}");
            if (!responseMessage.IsSuccessStatusCode)
                return Redirect("/Stadiums");

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var stadium = JsonConvert.DeserializeObject<ResultStadiumDto>(jsonData);
            if (stadium == null)
                return Redirect("/Stadiums");

            var model = new StadiumDetailViewModel { Stadium = stadium };

            // Bu stadyumda oynanan/oynanacak maçlar
            var fixturesResponse = await client.GetAsync($"{FixturesApi}/GetFixturesByStadium/{id}");
            if (fixturesResponse.IsSuccessStatusCode)
            {
                var fixturesJson = await fixturesResponse.Content.ReadAsStringAsync();
                model.Fixtures = JsonConvert.DeserializeObject<List<ResultFixtureDto>>(fixturesJson) ?? new();
            }

            return View(model);
        }
    }
}
