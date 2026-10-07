using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using SoccerProject.WebUI.DTOs.StadiumDtos;

namespace SoccerProject.WebUI.Controllers
{
    // "Admin - Stadyum Yönetimi" - API'yi (api/Stadiums) consume eder.
    public class StadiumController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Stadiums";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(ApiBase);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultStadiumDto>>(jsonData);
                return View(values);
            }
            return View(new List<ResultStadiumDto>());
        }

        [HttpGet]
        public IActionResult CreateStadium()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStadium(CreateStadiumDto createStadiumDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(createStadiumDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Stadyum eklendi.";
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
            return View(createStadiumDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStadium(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetStadiumById?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateStadiumDto>(jsonData);
                return View(value);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStadium(UpdateStadiumDto updateStadiumDto)
        {
            var client = new HttpClient();
            var jsonData = JsonConvert.SerializeObject(updateStadiumDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync(ApiBase, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Stadyum güncellendi.";
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
            return View(updateStadiumDto);
        }

        public async Task<IActionResult> DeleteStadium(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.DeleteAsync($"{ApiBase}?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Stadyum silindi.";
                return RedirectToAction("Index");
            }

            var errorMessage = await responseMessage.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(errorMessage) ? "Stadyum silinemedi." : errorMessage;
            return RedirectToAction("Index");
        }
    }
}
