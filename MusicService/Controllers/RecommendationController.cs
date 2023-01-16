using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Exceptions;
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
        public async Task<ActionResult<PlaylistWithTracksDTO>> GeneratePlaylist(List<TrackDTO> listTrack)
        {
            try{
                return await _service.GeneratePlaylist(listTrack);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }catch(BadRequestException e){
                return BadRequest(e.Content);
            }
        }   
    }
}