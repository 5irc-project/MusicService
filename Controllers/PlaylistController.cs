using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<List<PlaylistDTO>>> GetPlaylists()
        {
            return await _service.GetPlaylists();
        }

        // GET: api/Playlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylist(int id)
        {
            var pDTO = await _service.GetPlaylist(id);
            return pDTO == null ? NotFound() : pDTO;
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
            }catch(PlaylistNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Playlist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> PostPlaylist(PlaylistDTO pDTO)
        {
            try {
                await _service.PostPlaylist(pDTO);
            }catch(Exception){
                throw;
            }

            return CreatedAtAction("GetPlaylist", new { id = pDTO.PlaylistId }, pDTO);
        }

        // DELETE: api/Playlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            try{
                await _service.DeletePlaylist(id);
            }catch (PlaylistNotFoundException){
                return NotFound();
            }catch(Exception){
                throw;
            }
            return NoContent();
        }
    }
}
