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
        [HttpPost]
        public async Task<IActionResult> GeneratePlaylist(List<TrackDTO> listTrack)
        {
            await _service.DispatchRequestToQueue(listTrack, nameof(GeneratePlaylist));
            return Accepted();
        }   
    }
}