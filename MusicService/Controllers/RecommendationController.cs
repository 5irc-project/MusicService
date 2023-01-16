using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _service;

        public RecommendationController(IRecommendationService service)
        {
            _service = service;
        }

        // POST: api/Recommendation
        [HttpGet]
        public async Task<ActionResult<PlaylistWithTracksDTO>> GeneratePlaylist()
        {
            return await _service.GeneratePlaylist(new List<TrackDTO>());
        }
        
    }
}