using AutoMapper;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;

namespace MusicService.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;
        private readonly IPlaylistService _playlistService;


        public RecommendationService(MusicServiceDBContext context, IMapper mapper, ITrackService trackService, IPlaylistService playlistService)
        {
            _context = context;
            _mapper = mapper;
            _trackService = trackService;
            _playlistService = playlistService;
        }

        public async Task<PlaylistWithTracksDTO> GeneratePlaylist(List<TrackDTO> listTrack)
        {
            var rand = new Random();
            
            var g = _context.Genres.FirstOrDefault(g => g.GenreId == 1);

            if (g != null){
                GenreDTO gDTO = _mapper.Map<GenreDTO>(g);
                List<TrackWithGenresDTO> listTrackWithGenre = await _trackService.GetTracksByGenre(gDTO.GenreId);
                List<TrackWithGenresDTO> listTrackWithGenreRandom = listTrackWithGenre.OrderBy(x => rand.Next()).Take(20).ToList();
                var action = _playlistService.PostPlaylist(new PlaylistDTO() {
                    KindId = 2,
                    PlaylistName = "Testing",
                    UserId = 0
                });
                await _playlistService.AddTracksToPlaylist(action.Result.PlaylistId, _mapper.Map<List<TrackDTO>>(_mapper.Map<List<Track>>(listTrackWithGenreRandom)));
                return await _playlistService.GetPlaylist(action.Result.PlaylistId);
            }else{
                throw new NotFoundException("a", nameof(Genre));
            }
        }
    }
}