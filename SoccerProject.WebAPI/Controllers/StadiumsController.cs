using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.DTOs.StadiumDtos;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumsController : ControllerBase
    {
        private readonly SoccerDbContext _context;
        private readonly IMapper _mapper;

        public StadiumsController(SoccerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> StadiumList()
        {
            var values = await _context.Stadiums.ToListAsync();
            var result = _mapper.Map<List<ResultStadiumDto>>(values);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStadium(CreateStadiumDto createStadiumDto)
        {
            var stadium = _mapper.Map<Stadium>(createStadiumDto);
            await _context.Stadiums.AddAsync(stadium);
            await _context.SaveChangesAsync();
            return Ok("Stadyum eklendi");
        }

        [HttpGet("GetStadiumById")]
        public async Task<IActionResult> GetStadiumById(int id)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GetByIdStadiumDto>(stadium));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStadium(UpdateStadiumDto updateStadiumDto)
        {
            var stadium = await _context.Stadiums.FindAsync(updateStadiumDto.StadiumId);
            if (stadium == null)
            {
                return NotFound();
            }
            _mapper.Map(updateStadiumDto, stadium);
            await _context.SaveChangesAsync();
            return Ok("Stadyum güncellendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStadium(int id)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            _context.Stadiums.Remove(stadium);
            await _context.SaveChangesAsync();
            return Ok("Stadyum silindi");
        }
    }
}
