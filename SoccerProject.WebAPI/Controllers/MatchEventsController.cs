using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.MatchEventDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchEventsController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public MatchEventsController(IMapper mapper, SoccerDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MatchEventList()
        {
            var matchEvents = await _context.MatchEvents.ToListAsync();
            return Ok(matchEvents);
        }

        [HttpGet("GetMatchEventsByFixture/{id}")]
        public async Task<IActionResult> GetMatchEventsByFixture(int id)
        {
            var values = await _context.MatchEvents.Where(x => x.FixtureId == id).OrderBy(x => x.Minute).ToListAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatchEvent(CreateMatchEventDto createMatchEventDto)
        {
            var matchEvent = _mapper.Map<MatchEvent>(createMatchEventDto);
            await _context.MatchEvents.AddAsync(matchEvent);
            await _context.SaveChangesAsync();
            return Ok("Maç olayı eklendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatchEvent(int id)
        {
            var value = await _context.MatchEvents.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            _context.MatchEvents.Remove(value);
            await _context.SaveChangesAsync();
            return Ok("Maç olayı silindi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchEvent(int id)
        {
            var value = await _context.MatchEvents.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMatchEvent(UpdateMatchEventDto updateMatchEventDto)
        {
            var value = _mapper.Map<MatchEvent>(updateMatchEventDto);
            _context.MatchEvents.Update(value);
            await _context.SaveChangesAsync();
            return Ok("Maç olayı güncellendi");
        }


    }
}
