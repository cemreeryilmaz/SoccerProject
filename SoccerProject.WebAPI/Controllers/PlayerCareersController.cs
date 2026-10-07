using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.PlayerCareerDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerCareersController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public PlayerCareersController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Bir futbolcunun kariyer kayıtları
        [HttpGet("GetByPlayer/{playerId}")]
        public async Task<IActionResult> GetByPlayer(int playerId)
        {
            var values = await _context.PlayerCareers
                .Where(x => x.PlayerId == playerId)
                .OrderByDescending(x => x.StartYear)
                .ToListAsync();
            return Ok(_mapper.Map<List<ResultPlayerCareerDto>>(values));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayerCareer(CreatePlayerCareerDto dto)
        {
            var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == dto.PlayerId);
            if (!playerExists)
            {
                return BadRequest("Geçerli bir futbolcu seçmelisiniz.");
            }

            var career = _mapper.Map<PlayerCareer>(dto);
            await _context.PlayerCareers.AddAsync(career);
            await _context.SaveChangesAsync();
            return Ok("Kariyer kaydı eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlayerCareer(int id)
        {
            var career = await _context.PlayerCareers.FindAsync(id);
            if (career == null)
            {
                return NotFound();
            }
            _context.PlayerCareers.Remove(career);
            await _context.SaveChangesAsync();
            return Ok("Kariyer kaydı silindi");
        }
    }
}
