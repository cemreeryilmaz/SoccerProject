using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.TeamDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public TeamsController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> TeamList()
        {
            var values = await _context.Teams.ToListAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto createTeamDto)
        {
            // Serie A'da en fazla 20 takım olabilir.
            var teamCount = await _context.Teams.CountAsync();
            if (teamCount >= 20)
            {
                return BadRequest("Ligde en fazla 20 takım bulunabilir. Yeni takım eklemek için önce mevcut bir takımı silmelisiniz.");
            }

            var team = _mapper.Map<Team>(createTeamDto);
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return Ok("Takım eklendi");
        }

        [HttpGet("GetTeamById")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTeam(UpdateTeamDto updateTeamDto)
        {
            var team = await _context.Teams.FindAsync(updateTeamDto.TeamId);
            if (team == null)
            {
                return NotFound();
            }
            _mapper.Map(updateTeamDto, team);
            await _context.SaveChangesAsync();
            return Ok("Takım güncellendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            
            var hasMatches = await _context.Fixtures.AnyAsync(f => f.HomeTeamId == id || f.AwayTeamId == id);
            if (hasMatches)
            {
                return BadRequest($"'{team.TeamName}' takımının fikstürde maçları bulunduğu için silinemez. Önce bu takıma ait maçları silmelisiniz.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return Ok("Takım silindi");
        }
    }
}
