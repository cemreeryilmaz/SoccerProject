using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoccerProject.WebUI.DTOs.FixtureDtos;
using SoccerProject.WebUI.DTOs.TeamDtos;
using SoccerProject.WebUI.Enums;
using SoccerProject.WebUI.Models;

namespace SoccerProject.WebUI.Controllers
{
    public class StandingController : Controller
    {
        private const string ApiRoot = "https://localhost:7071/api";

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();

            var teams = new List<ResultTeamDto>();
            var teamsResponse = await client.GetAsync($"{ApiRoot}/Teams");
            if (teamsResponse.IsSuccessStatusCode)
            {
                var json = await teamsResponse.Content.ReadAsStringAsync();
                teams = JsonConvert.DeserializeObject<List<ResultTeamDto>>(json) ?? new();
            }

            var fixtures = new List<FixtureListDto>();
            var fixturesResponse = await client.GetAsync($"{ApiRoot}/Fixtures");
            if (fixturesResponse.IsSuccessStatusCode)
            {
                var json = await fixturesResponse.Content.ReadAsStringAsync();
                fixtures = JsonConvert.DeserializeObject<List<FixtureListDto>>(json) ?? new();
            }

            var model = BuildStandings(teams, fixtures);
            return View(model);
        }

        private static StandingsViewModel BuildStandings(List<ResultTeamDto> teams, List<FixtureListDto> fixtures)
        {
            var rows = teams.ToDictionary(
                t => t.TeamId,
                t => new StandingRowViewModel
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    LogoUrl = t.LogoUrl
                });

            var finished = fixtures
                .Where(f => f.MatchStatus == MatchStatus.Finished)
                .OrderBy(f => f.MatchDate)
                .ToList();

            foreach (var f in finished)
            {
                if (!rows.TryGetValue(f.HomeTeamId, out var home) ||
                    !rows.TryGetValue(f.AwayTeamId, out var away))
                    continue;

                home.Played++; away.Played++;
                home.GoalsFor += f.HomeTeamScore; home.GoalsAgainst += f.AwayTeamScore;
                away.GoalsFor += f.AwayTeamScore; away.GoalsAgainst += f.HomeTeamScore;

                if (f.HomeTeamScore > f.AwayTeamScore)
                {
                    home.Won++; home.Points += 3; home.Form.Add("G");
                    away.Lost++; away.Form.Add("M");
                }
                else if (f.HomeTeamScore < f.AwayTeamScore)
                {
                    away.Won++; away.Points += 3; away.Form.Add("G");
                    home.Lost++; home.Form.Add("M");
                }
                else
                {
                    home.Drawn++; home.Points++; home.Form.Add("B");
                    away.Drawn++; away.Points++; away.Form.Add("B");
                }
            }

            foreach (var r in rows.Values)
                if (r.Form.Count > 5)
                    r.Form = r.Form.Skip(r.Form.Count - 5).ToList();

            var ordered = rows.Values
                .OrderByDescending(r => r.Points)
                .ThenByDescending(r => r.GoalDifference)
                .ThenByDescending(r => r.GoalsFor)
                .ThenBy(r => r.TeamName)
                .ToList();

            int total = ordered.Count;
            for (int i = 0; i < total; i++)
            {
                var r = ordered[i];
                r.Position = i + 1;
                if (r.Position <= 4) r.Zone = "ucl";
                else if (r.Position <= 6) r.Zone = "europe";
                else if (r.Position >= total - 2) r.Zone = "rel";
                else r.Zone = "mid";
            }

            return new StandingsViewModel
            {
                Rows = ordered,
                TeamCount = total,
                Week = finished.Any() ? finished.Max(f => f.WeekNumber) : 0
            };
        }
    }
}
