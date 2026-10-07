using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.MatchStaticDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchStaticsController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public MatchStaticsController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> MatchStaticsList()
        {
            var values = await _context.MatchStatics.ToListAsync();
            return Ok(values);
        }

        [HttpGet("GetMatchStaticsByFixture/{id}")]
        public async Task<IActionResult> GetMatchStaticsByFixture(int id)
        {
            var value = await _context.MatchStatics.FirstOrDefaultAsync(x => x.FixtureId == id);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpGet("GetMatchStatics/{id}")]
        public async Task<IActionResult> GetMatchStatics(int id)
        {
            var value = await _context.MatchStatics.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatchStatics(CreateMatchStaticDto createMatchStaticsDto)
        {
            var matchStatics = _mapper.Map<MatchStatic>(createMatchStaticsDto);
            await _context.MatchStatics.AddAsync(matchStatics);
            await _context.SaveChangesAsync();
            return Ok("Maç istatistikleri eklendi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMatchStatics(UpdateMatchStaticDto updateMatchStaticsDto)
        {
            var value = _mapper.Map<MatchStatic>(updateMatchStaticsDto);
            _context.MatchStatics.Update(value);
            await _context.SaveChangesAsync();
            return Ok("Maç istatistikleri güncellendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMatchStatics(int id)
        {
            var value = await _context.MatchStatics.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            _context.MatchStatics.Remove(value);
            await _context.SaveChangesAsync();
            return Ok("Maç istatistikleri silindi");
        }
    }
}
