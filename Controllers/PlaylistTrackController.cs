using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistTrackController : ControllerBase
    {
        private readonly IPlaylistTrackService _service;

        public PlaylistTrackController(IPlaylistTrackService service)
        {
            _service = service;
        }

        // GET: api/PlaylistTrack
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistTrackDTO>>> GetAll()
        {
            return await _service.GetAll();
        }

        // GET: api/PlaylistTrack/Add/5
        [HttpPost("Add/{playlistId}")]
        public async Task<IActionResult> AddTracksToPlaylist(int playlistId, List<TrackDTO> lTD)
        {
            await _service.AddTracksToPlaylist(playlistId, lTD);
            return NoContent();
        }

        // PUT: api/PlaylistTrack/Remove/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Remove/{playlistId}")]
        public async Task<IActionResult> RemoveTracksFromPlaylist(int playlistId, List<TrackDTO> lTD)
        {
            await _service.RemoveTracksFromPlaylist(playlistId, lTD);
            return NoContent();
        }
    }
}
