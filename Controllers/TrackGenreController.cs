using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackGenreController : ControllerBase
    {
        private readonly ITrackGenreService _service;

        public TrackGenreController(ITrackGenreService service)
        {
            _service = service;
        }

        // GET: api/TrackGenre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackGenreDTO>>> GetAll()
        {
            return await _service.GetAll();
        }

        // POST: api/TrackGenre/Add/5
        [HttpPost("Add/{trackId}")]
        public async Task<IActionResult> AddGenresToTrack(int trackId, List<GenreDTO> lGD)
        {
            await _service.AddGenresToTrack(trackId, lGD);
            return NoContent();
        }

        // POST: api/TrackGenre/Remove/5     
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Remove/{trackId}")]
        public async Task<IActionResult> RemoveGenresFromTrack(int trackId, List<GenreDTO> lGD)
        {
            await _service.RemoveGenresFromTrack(trackId, lGD);
            return NoContent();
        }
    }
}
