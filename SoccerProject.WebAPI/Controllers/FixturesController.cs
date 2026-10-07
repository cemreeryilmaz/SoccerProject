using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.FixtureDtos;
using SoccerProject.WebAPI.Entities;
using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixturesController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public FixturesController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> FixtureList()
        {
            var values = await _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam).ToListAsync();
            return Ok(values);
        }

        [HttpGet("FixtureDropdown")]
        public async Task<IActionResult> FixtureDropdown()
        {
            var values = await _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam)
                .Select(x => new FixtureDropdownDto
                {
                    FixtureId = x.FixtureId,
                    HomeTeamName = x.HomeTeam.TeamName,
                    AwayTeamName = x.AwayTeam.TeamName,
                    WeekNumber = x.WeekNumber
                }).ToListAsync();
            return Ok(values);
        }

        [HttpGet("GetFixtureTeams/{id}")]
        public async Task<IActionResult> GetFixtureTeams(int id)
        {
            var value = await _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam).Where(x => x.FixtureId == id).Select(x => new 
            {
                HomeTeamId = x.HomeTeamId,
                HomeTeamName = x.HomeTeam.TeamName,
                AwayTeamId = x.AwayTeamId,
                AwayTeamName = x.AwayTeam.TeamName,
            }).FirstOrDefaultAsync();

            return Ok(value);
        }

        [HttpGet("GetFixturesByWeek/{weekNumber}")]
        public async Task<IActionResult> GetFixturesByWeek(int weekNumber)
        {
            var fixtures = await _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam)
                .Where(x => x.WeekNumber == weekNumber).ToListAsync();

            var finishedFixtures = await _context.Fixtures.Where(x => x.MatchStatus == MatchStatus.Finished).ToListAsync();

            var teamIds = finishedFixtures.SelectMany(x => new[] { x.HomeTeamId, x.AwayTeamId }).Distinct();

            var teamForms = new Dictionary<int, string[]>();

            foreach (var teamId in teamIds)
            {
                var form = finishedFixtures
                    .Where(x => x.HomeTeamId == teamId || x.AwayTeamId == teamId)
                    .OrderByDescending(x => x.MatchDate)
                    .Take(5)
                    .Select(x =>
                    {
                        bool isHome = x.HomeTeamId == teamId;

                        int teamScore = isHome ? x.HomeTeamScore : x.AwayTeamScore;
                        int opponentScore = isHome ? x.AwayTeamScore : x.HomeTeamScore;

                        if (teamScore > opponentScore) return "G";
                        if (teamScore < opponentScore) return "M";
                        return "B";
                    })
                    .ToArray();

                teamForms[teamId] = form;
            }


            var values = fixtures.Select(x => new ResultFixtureDto
            {
                FixtureId = x.FixtureId,
                HomeTeamId = x.HomeTeamId,
                AwayTeamId = x.AwayTeamId,
                HomeTeamName = x.HomeTeam.TeamName,
                AwayTeamName = x.AwayTeam.TeamName,
                HomeLogoUrl = x.HomeTeam.LogoUrl,
                AwayLogoUrl = x.AwayTeam.LogoUrl,
                HomeTeamHalfScore = x.HomeTeamHalfScore,
                AwayTeamHalfScore = x.AwayTeamHalfScore,
                HomeTeamScore = x.HomeTeamScore,
                AwayTeamScore = x.AwayTeamScore,
                Stadium = x.Stadium,
                MatchDate = x.MatchDate,
                Status = x.MatchStatus,
                HomeTeamForm = teamForms.TryGetValue(x.HomeTeamId, out var homeForm)
            ? homeForm
            : Array.Empty<string>(),

                AwayTeamForm = teamForms.TryGetValue(x.AwayTeamId, out var awayForm)
            ? awayForm
            : Array.Empty<string>()
            }).ToList();
            return Ok(values);
        }

        [HttpGet("GetFixturesByStadium/{stadiumId}")]
        public async Task<IActionResult> GetFixturesByStadium(int stadiumId)
        {
            var fixtures = await _context.Fixtures
                .Include(x => x.HomeTeam).Include(x => x.AwayTeam)
                .Where(x => x.StadiumId == stadiumId)
                .OrderBy(x => x.MatchDate)
                .Select(x => new ResultFixtureDto
                {
                    FixtureId = x.FixtureId,
                    HomeTeamId = x.HomeTeamId,
                    AwayTeamId = x.AwayTeamId,
                    HomeTeamName = x.HomeTeam.TeamName,
                    AwayTeamName = x.AwayTeam.TeamName,
                    HomeLogoUrl = x.HomeTeam.LogoUrl,
                    AwayLogoUrl = x.AwayTeam.LogoUrl,
                    HomeTeamHalfScore = x.HomeTeamHalfScore,
                    AwayTeamHalfScore = x.AwayTeamHalfScore,
                    HomeTeamScore = x.HomeTeamScore,
                    AwayTeamScore = x.AwayTeamScore,
                    Stadium = x.Stadium,
                    MatchDate = x.MatchDate,
                    Status = x.MatchStatus
                }).ToListAsync();
            return Ok(fixtures);
        }

        [HttpGet("GetFixtures")]
        public async Task<IActionResult> GetFixtures(MatchStatus? matchStatus, int? weekNumber)
        {
            var query = _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam).AsQueryable();

            if (matchStatus.HasValue)
            {
                query = query.Where(x => x.MatchStatus == matchStatus.Value);
            }
            if (weekNumber.HasValue)
            {
                query = query.Where(x => x.WeekNumber == weekNumber);
            }
            var result = await query.Select(x => new ResultFixtureDto
            {
                FixtureId = x.FixtureId,
                HomeTeamName = x.HomeTeam.TeamName,
                AwayTeamName = x.AwayTeam.TeamName,
                HomeLogoUrl = x.HomeTeam.LogoUrl,
                AwayLogoUrl = x.AwayTeam.LogoUrl,
                HomeTeamHalfScore = x.HomeTeamHalfScore,
                AwayTeamHalfScore = x.AwayTeamHalfScore,
                HomeTeamScore = x.HomeTeamScore,
                AwayTeamScore = x.AwayTeamScore,
                Stadium = x.Stadium,
                MatchDate = x.MatchDate,
                Status = x.MatchStatus
            }).ToListAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFixture(CreateFixtureDto createFixtureDto)
        {
            var value = _mapper.Map<Fixture>(createFixtureDto);
            await _context.Fixtures.AddAsync(value);
            await _context.SaveChangesAsync();
            return Ok("Fixture eklendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFixture(int id)
        {
            var value = await _context.Fixtures.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            _context.Fixtures.Remove(value);
            await _context.SaveChangesAsync();
            return Ok("Fixture silindi.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFixture(int id)
        {
            var value = await _context.Fixtures.Include(x => x.HomeTeam).Include(x => x.AwayTeam)
                                                .Include(x => x.MatchEvents).Include(x => x.MatchStatic)
                                                .FirstOrDefaultAsync(x => x.FixtureId == id);
            if (value == null)
            {
                return NotFound();
            }

            var mappedValue = _mapper.Map<GetByIdFixtureDto>(value);
            return Ok(mappedValue);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFixture(UpdateFixtureDto updateFixtureDto)
        {
            var value = _mapper.Map<Fixture>(updateFixtureDto);
            _context.Fixtures.Update(value);
            await _context.SaveChangesAsync();
            return Ok("Fixture güncellendi.");
        }
    }
}
