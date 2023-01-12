using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _service;

        public TrackController(ITrackService service)
        {
            _service = service;
        }

        // GET: api/Track
        [HttpGet]
        public async Task<ActionResult<List<TrackDTO>>> GetTracks()
        {
            return await _service.GetTracks();
        }

        // GET: api/Track/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDTO?>> GetTrack(int id)
        {
            var tDTO = await _service.GetTrack(id);
            return tDTO == null ? NotFound() : tDTO;
        }

        // PUT: api/Track/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(int id, TrackDTO tDTO)
        {
            if (id != tDTO.TrackId) {
                return BadRequest();
            }
            
            try{
                await _service.PutTrack(id, tDTO);
            }catch(TrackNotFoundException){
                return NotFound();
            }catch(GenreNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Track
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackDTO>> PostTrack(TrackDTO tDTO)
        {
            try {
                await _service.PostTrack(tDTO);
            }catch(GenreNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }

            return CreatedAtAction("GetTrack", new { id = tDTO.TrackId }, tDTO);
        }

        // DELETE: api/Track/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try{
                await _service.DeleteTrack(id);
            }catch (TrackNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
            return NoContent();
        }
    }
}
