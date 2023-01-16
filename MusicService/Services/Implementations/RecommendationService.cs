using AutoMapper;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Message;
using MusicService.Message.Interfaces;
using MusicService.Models;
using MusicService.RestConsumer;
using MusicService.Services.Interfaces;

namespace MusicService.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;
        private readonly IPlaylistService _playlistService;
        private readonly IMessageProducer _messageProducer;
        private readonly MessageConsumer<RecommendationService> _messageConsumer;

        public RecommendationService(MusicServiceDBContext context, IMapper mapper, ITrackService trackService, IPlaylistService playlistService, IMessageProducer messageProducer)
        {
            _context = context;
            _mapper = mapper;
            _trackService = trackService;
            _playlistService = playlistService;
            _messageProducer = messageProducer;
            _messageConsumer = new MessageConsumer<RecommendationService>(this);
        }

        public async Task DispatchRequestToQueue(List<TrackDTO> listTrack, string nameOfMethod){
            _messageProducer.ProduceMessage(new QueueMessage(listTrack, nameOfMethod));
        }

        public async Task GeneratePlaylist(List<TrackDTO> listTrack)
        {
            try{
                var rand = new Random();
                if (listTrack.Count == 0){
                    throw new BadRequestException("Please provide at least ten tracks");
                }

                var genrePredicted = await MLService.PredictGenre(_mapper.Map<List<TrackMachineLearningDTO>>(listTrack));
                if (genrePredicted == null){
                    throw new BadRequestException("Couldn't predict a genre");
                }

                var g = _context.Genres.FirstOrDefault(g => g.Name == genrePredicted.Name);
                if (g == null){
                    throw new NotFoundException(genrePredicted.Name, nameof(Genre));
                }
                
                GenreDTO gDTO = _mapper.Map<GenreDTO>(g);
                List<TrackWithGenresDTO> listTrackWithGenre = await _trackService.GetTracksByGenre(gDTO.GenreId);
                List<TrackWithGenresDTO> listTrackWithGenreRandom = listTrackWithGenre.OrderBy(x => rand.Next()).Take(20).ToList();
                var action = _playlistService.PostPlaylist(new PlaylistDTO() {
                    KindId = 1,
                    PlaylistName = "Testing",
                    UserId = 0
                });
                await _playlistService.AddTracksToPlaylist(action.Result.PlaylistId, _mapper.Map<List<TrackDTO>>(_mapper.Map<List<Track>>(listTrackWithGenreRandom)));
            }catch(Exception e){
                throw e;
            }
        }
    }
}