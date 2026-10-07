using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.PlayerDtos;
using SoccerProject.WebAPI.Entities;
using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public PlayersController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> PlayerList()
        {
            // Takım adını da döndürebilmek için Team'i dahil ediyoruz.
            var values = await _context.Players.Include(x => x.Team).ToListAsync();
            var result = _mapper.Map<List<ResultPlayerDto>>(values);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(CreatePlayerDto createPlayerDto)
        {
            // Seçilen takım gerçekten var mı?
            var teamExists = await _context.Teams.AnyAsync(t => t.TeamId == createPlayerDto.TeamId);
            if (!teamExists)
            {
                return BadRequest("Geçerli bir takım seçmelisiniz.");
            }

            var player = _mapper.Map<Player>(createPlayerDto);
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            return Ok("Futbolcu eklendi");
        }

        [HttpGet("GetPlayerById")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            // Takım adı + kariyer geçmişi ile birlikte getir.
            var player = await _context.Players
                .Include(x => x.Team)
                .Include(x => x.Careers)
                .FirstOrDefaultAsync(x => x.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<GetByIdPlayerDto>(player);
            dto.Careers = dto.Careers.OrderByDescending(c => c.StartYear).ToList();
            return Ok(dto);
        }

        // Belirli bir takımın futbolcuları (public takım/oyuncu sayfaları için)
        [HttpGet("GetPlayersByTeam/{teamId}")]
        public async Task<IActionResult> GetPlayersByTeam(int teamId)
        {
            var values = await _context.Players
                .Include(x => x.Team)
                .Where(x => x.TeamId == teamId)
                .OrderBy(x => x.ShirtNumber)
                .ToListAsync();
            return Ok(_mapper.Map<List<ResultPlayerDto>>(values));
        }

        // Gol krallığı: futbolcuların attığı gollerin toplamı (Goal=0, PenaltyGoal=7)
        [HttpGet("GetTopScorers")]
        public async Task<IActionResult> GetTopScorers(int take = 50)
        {
            var goalTypes = new[] { MatchEventType.Goal, MatchEventType.PenaltyGoal };

            var grouped = await _context.MatchEvents
                .Where(e => e.PlayerId != null && goalTypes.Contains(e.MatchEventType))
                .GroupBy(e => e.PlayerId!.Value)
                .Select(g => new { PlayerId = g.Key, Goals = g.Count() })
                .OrderByDescending(x => x.Goals)
                .Take(take)
                .ToListAsync();

            var ids = grouped.Select(x => x.PlayerId).ToList();
            var players = await _context.Players
                .Include(p => p.Team)
                .Where(p => ids.Contains(p.PlayerId))
                .ToListAsync();

            var result = grouped
                .Select(x =>
                {
                    var p = players.First(pl => pl.PlayerId == x.PlayerId);
                    return new TopScorerDto
                    {
                        PlayerId = p.PlayerId,
                        FullName = p.FullName,
                        Position = p.Position,
                        PhotoUrl = p.PhotoUrl,
                        Nationality = p.Nationality,
                        TeamId = p.TeamId,
                        TeamName = p.Team?.TeamName,
                        Goals = x.Goals,
                    };
                })
                .OrderByDescending(x => x.Goals)
                .ThenBy(x => x.FullName)
                .ToList();

            // Sıralama numarası (aynı gol sayısı aynı sıra)
            int rank = 0, prevGoals = -1, seen = 0;
            foreach (var row in result)
            {
                seen++;
                if (row.Goals != prevGoals)
                {
                    rank = seen;
                    prevGoals = row.Goals;
                }
                row.Rank = rank;
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePlayer(UpdatePlayerDto updatePlayerDto)
        {
            var player = await _context.Players.FindAsync(updatePlayerDto.PlayerId);
            if (player == null)
            {
                return NotFound();
            }

            var teamExists = await _context.Teams.AnyAsync(t => t.TeamId == updatePlayerDto.TeamId);
            if (!teamExists)
            {
                return BadRequest("Geçerli bir takım seçmelisiniz.");
            }

            _mapper.Map(updatePlayerDto, player);
            await _context.SaveChangesAsync();
            return Ok("Futbolcu güncellendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok("Futbolcu silindi");
        }
    }
}
