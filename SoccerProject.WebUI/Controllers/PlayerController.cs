using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using SoccerProject.WebUI.DTOs.PlayerDtos;
using SoccerProject.WebUI.DTOs.PlayerCareerDtos;
using SoccerProject.WebUI.DTOs.TeamDtos;

namespace SoccerProject.WebUI.Controllers
{
    // "Admin - Futbolcu Yönetimi" - API'yi (api/Players) consume eder.
    public class PlayerController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Players";
        private const string TeamsApi = "https://localhost:7071/api/Teams";
        private const string CareersApi = "https://localhost:7071/api/PlayerCareers";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(ApiBase);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPlayerDto>>(jsonData);
                return View(values);
            }
            return View(new List<ResultPlayerDto>());
        }

        [HttpGet]
        public async Task<IActionResult> CreatePlayer()
        {
            await LoadTeamsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(CreatePlayerDto createPlayerDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(createPlayerDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Futbolcu eklendi.";
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
            await LoadTeamsAsync();
            return View(createPlayerDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePlayer(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetPlayerById?id={id}");
            if (!responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdatePlayerDto>(jsonData);

            await LoadTeamsAsync();
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlayer(UpdatePlayerDto updatePlayerDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(updatePlayerDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Futbolcu güncellendi.";
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
            await LoadTeamsAsync();
            return View(updatePlayerDto);
        }

        public async Task<IActionResult> DeletePlayer(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.DeleteAsync($"{ApiBase}?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Futbolcu silindi.";
                return RedirectToAction("Index");
            }

            var errorMessage = await responseMessage.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage) ? "Futbolcu silinemedi." : errorMessage;
            return RedirectToAction("Index");
        }

        // ---------- Kariyer (oynadığı kulüpler) yönetimi ----------
        [HttpGet]
        public async Task<IActionResult> Careers(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetPlayerById?id={id}");
            if (!responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var player = JsonConvert.DeserializeObject<GetByIdPlayerDto>(jsonData);
            if (player == null)
                return RedirectToAction("Index");

            return View(player);
        }

        [HttpPost]
        public async Task<IActionResult> AddCareer(CreatePlayerCareerDto dto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(CareersApi, content);
            if (responseMessage.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Kariyer kaydı eklendi.";
            else
                TempData["ErrorMessage"] = await responseMessage.Content.ReadAsStringAsync();

            return RedirectToAction("Careers", new { id = dto.PlayerId });
        }

        public async Task<IActionResult> DeleteCareer(int id, int playerId)
        {
            var client = new HttpClient();
            var responseMessage = await client.DeleteAsync($"{CareersApi}?id={id}");
            if (responseMessage.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Kariyer kaydı silindi.";
            else
                TempData["ErrorMessage"] = "Kariyer kaydı silinemedi.";

            return RedirectToAction("Careers", new { id = playerId });
        }

        // Create/Update formlarındaki takım dropdown'u için
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
        }
    }
}
