using Microsoft.AspNetCore.Authorization;
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

        // GET: api/Track/Random
        [HttpGet("Random")]
        public async Task<ActionResult<TrackWithGenresDTO?>> GetRandomTrack()
        {
            return await _service.GetRandomTrack();
        }

        // GET: api/Track/NameQuery/"ex"
        [HttpGet("NameQuery/{nameQuery}")]
        public async Task<ActionResult<List<TrackWithGenresDTO>>> GetTracksByNameQuery(string nameQuery)
        {
            return await _service.GetTracksByNameQuery(nameQuery);
        }

        // GET: api/Track/NameQuery/"ex"
        [HttpGet("Genre/{genreId}")]
        public async Task<ActionResult<List<TrackWithGenresDTO>>> GetTracskByGenre(int genreId)
        {
            try{
                return await _service.GetTracksByGenre(genreId);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
        }

        // // PUT: api/Track/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTrack(int id, TrackDTO tDTO)
        // {
        //     if (id != tDTO.TrackId) {
        //         return BadRequest();
        //     }
            
        //     try{
        //         await _service.PutTrack(id, tDTO);
        //     }catch(NotFoundException e){
        //         return NotFound(e.Content);
        //     }
    
        //     return NoContent();
        // }

        // // POST: api/Track
        // [HttpPost]
        // public async Task<ActionResult<TrackWithGenresDTO>> PostTrack(TrackDTO tDTO)
        // {
        //     try{
        //         TrackDTO track = await _service.PostTrack(tDTO);
        //         return CreatedAtAction("GetTrack", new { id = track.TrackId }, track);
        //     }catch(AlreadyExistsException e){
        //         return BadRequest(e.Content);
        //     }

        // }

        // // POST: api/Track/TrackGenre
        // [HttpPost("TrackGenre")]
        // public async Task<ActionResult<TrackWithGenresDTO>> PostTrackWithGenres(TrackWithGenresDTO twgDTO)
        // {
        //     try{
        //         TrackDTO track = await _service.PostTrackWithGenres(twgDTO);
        //         return CreatedAtAction("GetTrack", new { id = track.TrackId }, track);
        //     }catch(NotFoundException e){
        //         return NotFound(e.Content);
        //     }catch(AlreadyExistsException e){
        //         return BadRequest(e.Content);
        //     }
        // }

        // // DELETE: api/Track/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteTrack(int id)
        // {
        //     try{
        //         await _service.DeleteTrack(id);
        //     }catch (NotFoundException e){
        //         return NotFound(e.Content);
        //     }
            
        //     return NoContent();
        // }

        // // POST: api/Track/TrackGenre/Add/5
        // [HttpPost("TrackGenre/Add/{id}")]
        // public async Task<IActionResult> AddGenresToTrack(int id, List<GenreDTO> lGD)
        // {
        //     try{
        //         await _service.AddGenresToTrack(id, lGD);
        //     }catch(NotFoundException e){
        //         return NotFound(e.Content);
        //     }
        //     return NoContent();
        // }

        // // POST: api/Track/TrackGenre/Remove/5     
        // [HttpPost("TrackGenre/Remove/{id}")]
        // public async Task<IActionResult> RemoveGenresFromTrack(int id, List<GenreDTO> lGD)
        // {
        //     try{
        //         await _service.RemoveGenresFromTrack(id, lGD);
        //     }catch(NotFoundException e){
        //         return NotFound(e.Content);
        //     }
        //     return NoContent();
        // }

        [Authorize]
        [HttpGet("{id}/IsInFavorite/")]
        public async Task<ActionResult<bool>> IsTrackInFavorite(int id){
            var userId = GetUserIdFromClaims();
            try{
                return await _service.IsTrackInFavorite(id, userId);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
        }

        [HttpGet("{id}/IsInPlaylist/{playlistId}")]
        public async Task<ActionResult<bool>> IsTrackInPlaylist(int id, int playlistId){
            try{
                return await _service.IsTrackInPlaylist(id, playlistId);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
        }

        private int GetUserIdFromClaims() {
            var id = User.Claims.First(c => c.Type == "UserId").Value;
            return int.Parse(id);
        }        
    }
}
