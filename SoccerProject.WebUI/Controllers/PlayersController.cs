using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerProject.WebUI.DTOs.PlayerDtos;

namespace SoccerProject.WebUI.Controllers
{
    // Herkese açık (ön yüz) Futbolcu sayfaları - /Players
    public class PlayersController : Controller
    {
        private const string ApiBase = "https://localhost:7071/api/Players";

        // Tüm futbolcular (takıma göre gruplanarak gösterilir)
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(ApiBase);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultPlayerDto>>(jsonData) ?? new();
                return View(values);
            }
            return View(new List<ResultPlayerDto>());
        }

        // Gol krallığı tablosu
        public async Task<IActionResult> TopScorers()
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetTopScorers?take=50");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<TopScorerDto>>(jsonData) ?? new();
                return View(values);
            }
            return View(new List<TopScorerDto>());
        }

        // Futbolcu profil detayı (foto, yaş, uyruk, kariyer)
        public async Task<IActionResult> Detail(int id)
        {
            var client = new HttpClient();
            var responseMessage = await client.GetAsync($"{ApiBase}/GetPlayerById?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<GetByIdPlayerDto>(jsonData);
                if (value != null)
                    return View(value);
            }
            return Redirect("/Players");
        }
    }
}
