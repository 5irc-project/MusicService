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
        public async Task<ActionResult<List<TrackWithGenresDTO>>> GetTracks()
        {
            return await _service.GetTracks();
        }

        // GET: api/Track/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackWithGenresDTO?>> GetTrack(int id)
        {
            try{
                return await _service.GetTrack(id);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
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
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
    
            return NoContent();
        }

        // POST: api/Track
        [HttpPost]
        public async Task<ActionResult<TrackWithGenresDTO>> PostTrack(TrackDTO tDTO)
        {
            try{
                await _service.PostTrack(tDTO);
            }catch(AlreadyExistsException e){
                return BadRequest(e.Content);
            }

            return CreatedAtAction("GetTrack", new { id = tDTO.TrackId }, tDTO);
        }

        // POST: api/Track/TrackGenre
        [HttpPost("TrackGenre")]
        public async Task<ActionResult<TrackWithGenresDTO>> PostTrackWithGenres(TrackWithGenresDTO twgDTO)
        {
            try{
                await _service.PostTrackWithGenres(twgDTO);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }catch(AlreadyExistsException e){
                return BadRequest(e.Content);
            }

            return CreatedAtAction("GetTrack", new { id = twgDTO.TrackId }, twgDTO);
        }

        // DELETE: api/Track/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try{
                await _service.DeleteTrack(id);
            }catch (NotFoundException e){
                return NotFound(e.Content);
            }
            
            return NoContent();
        }

        // POST: api/Track/TrackGenre/Add/5
        [HttpPost("TrackGenre/Add/{id}")]
        public async Task<IActionResult> AddGenresToTrack(int id, List<GenreDTO> lGD)
        {
            try{
                await _service.AddGenresToTrack(id, lGD);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
            return NoContent();
        }

        // POST: api/Track/TrackGenre/Remove/5     
        [HttpPost("TrackGenre/Remove/{id}")]
        public async Task<IActionResult> RemoveGenresFromTrack(int id, List<GenreDTO> lGD)
        {
            try{
                await _service.RemoveGenresFromTrack(id, lGD);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
            return NoContent();
        }
    }
}
