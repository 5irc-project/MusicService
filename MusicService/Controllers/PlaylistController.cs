using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Message;
using MusicService.Services.Implementations;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _service;
        private readonly IPublishEndpoint _publishEndPoint;

        public PlaylistController(IPlaylistService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            _publishEndPoint = publishEndpoint;
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
        public async Task<ActionResult<List<PlaylistWithTracksDTO>>> GetPlaylistsByUserId(int userId)
        {
            return await _service.GetPlaylistsByUserId(userId);
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
                PlaylistDTO playlist = await _service.PostPlaylist(pDTO);
                return CreatedAtAction("GetPlaylist", new { id = playlist.PlaylistId }, playlist);
            }catch(AlreadyExistsException e){
                return BadRequest(e.Content);
            }catch(BadRequestException e){
                return BadRequest(e.Content);
            }
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

        [HttpPost("PlaylistTrack/Add/Favorite")]
        [Authorize]
        public async Task<IActionResult> AddTrackToFavoritePlaylist(TrackDTO trackDTO)
        {
            // Check user exists ?
            try{
                var userId = GetUserIdFromClaims();
                await _service.AddTrackToFavoritePlaylist(userId, trackDTO);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }

            return NoContent();
        }

        [HttpPost("PlaylistTrack/Remove/Favorite")]
        [Authorize]
        public async Task<IActionResult> RemoveTrackFromFavoritePlaylist(TrackDTO trackDTO)
        {
            // Check user exists ?
            try{
                var userId = GetUserIdFromClaims();
                await _service.RemoveTrackFromFavoritePlaylist(userId, trackDTO);
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

        // POST: api/Recommendation
        [HttpPost("Recommendation")]
        public IActionResult GeneratePlaylist(List<TrackDTO> listTrack)
        {
            _publishEndPoint.Publish<MessageQueue>(new MessageQueue(listTrack));
            return Accepted();
        }

        // POST: api/Recommendation
        [Authorize]
        [HttpPost("Recommendation/Dev")]
        public async Task<IActionResult> GeneratePlaylistDev(List<TrackDTO> listTrack) // TO REMOVE
        {
            try{
                var userId = GetUserIdFromClaims();
                PlaylistDTO pCreated = await _service.GeneratePlaylistDev(listTrack, userId);
                return CreatedAtAction("GetPlaylist", new { id = pCreated.PlaylistId }, pCreated);
            }catch(BadRequestException e){
                return BadRequest(e.Content);
            }
        }

        // POST: api/Playlist/private/{userId}
        [HttpPost("private/{userId}")]
        public async Task<ActionResult<PlaylistDTO>> AddFavoritePlaylist(int userId)
        {
            try{
                PlaylistDTO playlist = await _service.AddFavoritePlaylist(userId);
                return CreatedAtAction("GetPlaylist", new { id = playlist.PlaylistId }, playlist);
            }catch(AlreadyExistsException e){
                return BadRequest(e.Content);
            }
        }  

        // POST: api/Playlist/private/5
        [HttpDelete("private/{userId}")]
        public async Task<IActionResult> DeletePlaylists(int userId)
        {
            await _service.DeletePlaylists(userId);
            return NoContent();
        }  

        // Get: api/Track/Playlists     
        [Authorize]
        [HttpGet("Trackless/{trackId}")]
        public async Task<ActionResult<List<PlaylistDTO>>> GetPlaylistsWithoutTrackForUser(int trackId)
        {
            try{
                var userId = GetUserIdFromClaims();
                return await _service.GetPlaylistsWithoutTrackForUser(trackId, userId);
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
