using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.Exceptions;
using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService _service;

        public MoodController(IMoodService service)
        {
            _service = service;
        }

        // GET: api/Mood
        [HttpGet]
        public async Task<ActionResult<List<MoodDTO>>> GetMoods()
        {
            return await _service.GetMoods();
        }

        // GET: api/Mood/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoodDTO>> GetMood(int id)
        {
            var mDTO = await _service.GetMood(id);
            return mDTO == null ? NotFound() : mDTO;
        }

        // PUT: api/Mood/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMood(int id, MoodDTO mDTO)
        {
            if (id != mDTO.MoodId) {
                return BadRequest();
            }
            
            try{
                await _service.PutMood(id, mDTO);
            }catch(MoodNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Mood
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MoodDTO>> PostMood(MoodDTO mDTO)
        {
            try {
                await _service.PostMood(mDTO);
            }catch(Exception){
                throw;
            }

            return CreatedAtAction("GetMusic", new { id = mDTO.MoodId }, mDTO);
        }

        // DELETE: api/Mood/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMood(int id)
        {
            try{
                await _service.DeleteMood(id);
            }catch (MoodNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
            return NoContent();
        }
    }
}
