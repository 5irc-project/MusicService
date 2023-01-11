using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _service;

        public MusicController(IMusicService service)
        {
            _service = service;
        }

        // GET: api/Music
        [HttpGet]
        public async Task<ActionResult<List<MusicDTO>>> GetMusics()
        {
            return await _service.GetMusics();
        }

        // GET: api/Music/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicDTO?>> GetMusic(int id)
        {
            var mDTO = await _service.GetMusic(id);
            return mDTO == null ? NotFound() : mDTO;
        }

        // PUT: api/Music/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusic(int id, MusicDTO mDTO)
        {
            if (id != mDTO.MusicId) {
                return BadRequest();
            }
            
            try{
                await _service.PutMusic(id, mDTO);
            }catch(MusicNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Music
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Music>> PostMusic(MusicDTO mDTO)
        {
            try {
                await _service.PostMusic(mDTO);
            }catch(Exception){
                throw;
            }

            return CreatedAtAction("GetMusic", new { id = mDTO.MusicId }, mDTO);
        }

        // DELETE: api/Music/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            try{
                await _service.DeleteMusic(id);
            }catch (MusicNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
            return NoContent();
        }
    }
}
