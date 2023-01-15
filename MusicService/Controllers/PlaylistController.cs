using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _service;

        public PlaylistController(IPlaylistService service)
        {
            _service = service;
        }

        // GET: api/Playlist
        [HttpGet]
        public async Task<ActionResult<List<PlaylistWithTracksDTO>>> GetPlaylists()
        {
            return await _service.GetPlaylists();
        }

        // GET: api/Playlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistWithTracksDTO>> GetPlaylist(int id)
        {
            try{
                return await _service.GetPlaylist(id);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
        }

        // GET: api/Playlist/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<List<PlaylistWithTracksDTO>>> GetPlaylistsByUser(int userId)
        {
            return await _service.GetPlaylistsByUser(userId);
        }

        // PUT: api/Playlist/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist(int id, PlaylistDTO pDTO)
        {
            if (id != pDTO.PlaylistId) {
                return BadRequest();
            }
            
            try{
                await _service.PutPlaylist(id, pDTO);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
    
            return NoContent();
        }

        // POST: api/Playlist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> PostPlaylist(PlaylistDTO pDTO)
        {
            try{
                await _service.PostPlaylist(pDTO);
            }catch(AlreadyExistsException e){
                return BadRequest(e.Content);
            }
            return CreatedAtAction("GetPlaylist", new { id = pDTO.PlaylistId }, pDTO);
        }

        // DELETE: api/Playlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            try{
                await _service.DeletePlaylist(id);
            }catch (NotFoundException e){
                return NotFound(e.Content);
            }
            
            return NoContent();
        }

        // GET: api/Playlist/PlaylistTrack/Add/5
        [HttpPost("PlaylistTrack/Add/{id}")]
        public async Task<IActionResult> AddTracksToPlaylist(int id, List<TrackDTO> lTD)
        {
            try{
                await _service.AddTracksToPlaylist(id, lTD);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }

            return NoContent();
        }

        // PUT: api/Playlist/PlaylistTrack/Remove/5
        [HttpPost("PlaylistTrack/Remove/{id}")]
        public async Task<IActionResult> RemoveTracksFromPlaylist(int id, List<TrackDTO> lTD)
        {
            try{
                await _service.RemoveTracksFromPlaylist(id, lTD);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }

            return NoContent();
        }
    }
}
