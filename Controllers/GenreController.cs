using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
            _service = service;
        }

        // GET: api/Genre
        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            return await _service.GetGenres();
        }

        // GET: api/Genre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            var gDTO = await _service.GetGenre(id);
            return gDTO == null ? NotFound() : gDTO;
        }

        // PUT: api/Genre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreDTO gDTO)
        {
            if (id != gDTO.GenreId) {
                return BadRequest();
            }
            
            try{
                await _service.PutGenre(id, gDTO);
            }catch(NotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Genre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenreDTO gDTO)
        {
            try {
                await _service.PostGenre(gDTO);
            }catch(Exception){
                throw;
            }
            return CreatedAtAction("GetGenre", new { id = gDTO.GenreId }, gDTO);
        }

        // DELETE: api/Genre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
             try{
                await _service.DeleteGenre(id);
            }catch (NotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
            return NoContent();
        }
    }
}
