using AutoMapper;
using MusicService.DTOs;
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
            // TODO : Check the genre exists, check the playlist was created, check the tracks aren't empty

            var rand = new Random();
            GenreDTO genreFromPython = _mapper.Map<GenreDTO>(_context.Genres.Find(1));
            List<TrackWithGenresDTO> listTrackWithGenre = await _trackService.GetTracksByGenre(genreFromPython.GenreId);
            List<TrackWithGenresDTO> listTrackWithGenreRandom = listTrackWithGenre.OrderBy(x => rand.Next()).Take(20).ToList();

            var action = _playlistService.PostPlaylist(new PlaylistDTO() {
                KindId = 2,
                PlaylistName = "Testing",
                UserId = 0
            });

            await _playlistService.AddTracksToPlaylist(action.Result.PlaylistId, _mapper.Map<List<TrackDTO>>(_mapper.Map<List<Track>>(listTrackWithGenreRandom)));

            return await _playlistService.GetPlaylist(action.Result.PlaylistId);
        }
    }
}