using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using SoccerProject.WebUI.DTOs.TeamDtos;

namespace SoccerProject.WebUI.Controllers
{
    public class TeamController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Teams";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(ApiBase);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultTeamDto>>(jsonData);
                return View(values);
            }

            return View(new List<ResultTeamDto>());
        }

        [HttpGet]
        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto createTeamDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(createTeamDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var errorMessage = await responseMessage.Content.ReadAsStringAsync();
            ViewBag.ErrorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Takım eklenirken bir hata oluştu."
                : errorMessage;
            return View(createTeamDto);
        }

        public async Task<IActionResult> DeleteTeam(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.DeleteAsync($"{ApiBase}?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Takım silindi.";
                return RedirectToAction("Index");
            }

            var errorMessage = await responseMessage.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage)
                ? "Takım silinemedi."
                : errorMessage;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTeam(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetTeamById?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateTeamDto>(jsonData);
                return View(value);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeam(UpdateTeamDto updateTeamDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(updateTeamDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(updateTeamDto);
        }
    }
}
