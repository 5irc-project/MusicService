using MusicService.Models;
using MusicService.Exceptions;
using MusicService.DTOs;
using MusicService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using MusicService.Message;
using MusicService.HttpClient;

namespace MusicService.Services.Implementations
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IConfiguration _config;
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;
        private readonly ITrackService _trackService;
        private readonly IMLHttpClient _MLHttpClient;
        private IBus _bus;

        public PlaylistService(MusicServiceDBContext context, IMapper mapper, ITrackService trackService, IConfiguration config, IBus bus, IMLHttpClient MLHttpClient)
        {
            _context = context;
            _mapper = mapper;
            _trackService = trackService;
            _config = config;
            _bus = bus;
            _MLHttpClient = MLHttpClient;
        }

        public async Task DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                    throw new NotFoundException(id, nameof(Playlist));
            }
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<PlaylistWithTracksDTO> GetPlaylist(int id)
        {
            Playlist? p = await _context.Playlists
                .Include(p => p.Kind)
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.PlaylistId == id);

            if (p == null){
                throw new NotFoundException(id, nameof(Playlist));
            }
            PlaylistWithTracksDTO pwtDTO = _mapper.Map<PlaylistWithTracksDTO>(p);
            pwtDTO.Tracks = new List<TrackWithGenresDTO>();
            #pragma warning disable CS8602
            foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                pwtDTO.Tracks.Add(_trackService.GetTrack(playlistTrack.TrackId).Result);
            }
            #pragma warning restore CS8602  
            return pwtDTO;
        }

        public async Task<List<PlaylistWithTracksDTO>> GetPlaylistsByUserId(int userId)
        {
            List<Playlist>? listPlaylist = await _context.Playlists
                .Include(p => p.Kind)
                .Include(p => p.PlaylistTracks)
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
            List<PlaylistWithTracksDTO> listPlaylistWithTrack = new List<PlaylistWithTracksDTO>();

            listPlaylist.ForEach(p => {
                PlaylistWithTracksDTO pwtDTO = _mapper.Map<PlaylistWithTracksDTO>(p);
                pwtDTO.Tracks = new List<TrackWithGenresDTO>();
                #pragma warning disable CS8602
                foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                    pwtDTO.Tracks.Add(_trackService.GetTrack(playlistTrack.TrackId).Result);
                }
                #pragma warning restore CS8602  
                listPlaylistWithTrack.Add(pwtDTO);
            });
            return listPlaylistWithTrack;
        }

        public async Task<List<PlaylistWithTracksDTO>> GetPlaylists()
        {
            List<Playlist> lP = await _context.Playlists
                        .Include(p => p.Kind)
                        .Include(p => p.PlaylistTracks)
                        .ToListAsync();
            List<PlaylistWithTracksDTO> lpwtDTO = new List<PlaylistWithTracksDTO>();
            lP.ForEach(p => {
                PlaylistWithTracksDTO pwtDTO = _mapper.Map<PlaylistWithTracksDTO>(p);
                pwtDTO.Tracks = new List<TrackWithGenresDTO>();
                #pragma warning disable CS8602
                foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                    pwtDTO.Tracks.Add(
                       _trackService.GetTrack(playlistTrack.TrackId).Result
                    );
                }
                #pragma warning restore CS8602
                lpwtDTO.Add(pwtDTO);

            });
            return lpwtDTO;        
        }

        public async Task<PlaylistDTO> PostPlaylist(PlaylistDTO pDTO)
        {
            // Check if user exists instead !
            if (pDTO.UserId == 0){
                throw new BadRequestException("Please provide a user Id in your request");
            }

            if (_context.Playlists.FirstOrDefault(p => p.PlaylistId == pDTO.PlaylistId) != null){
                throw new AlreadyExistsException(nameof(Playlist), pDTO.PlaylistId);
            }

            Playlist playlistToAdd = _mapper.Map<Playlist>(pDTO);
            _context.Playlists.Add(playlistToAdd);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlaylistDTO>(playlistToAdd);
        }

        public async Task PutPlaylist(int id, PlaylistDTO pDTO)
        {
            if (!_context.Playlists.Any(e => e.PlaylistId == id)){
                    throw new NotFoundException(id, nameof(Playlist));
            }
            _context.Entry(_mapper.Map<Playlist>(pDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddTracksToPlaylist(int id, List<TrackDTO> lTD)
        {
            Playlist? p = await _context.Playlists.FirstOrDefaultAsync(p => p.PlaylistId == id);
            
            if (p != null){
                // If every genre exists and the track isn't null
                if (lTD.All(tDTO => _context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null) == true && p != null){
                    foreach(var tDTO in lTD){
                        Track? t = await _context.Tracks.FirstOrDefaultAsync(t => t.TrackId == tDTO.TrackId);
                        #pragma warning disable CS8601
                        if (_context.PlaylistTracks.FirstOrDefault(pt => pt.PlaylistId == id && pt.TrackId == tDTO.TrackId) == null){
                            _context.PlaylistTracks.Add(new PlaylistTrack {
                                TrackId = tDTO.TrackId,
                                Track = t,
                                PlaylistId = id,
                                Playlist = p
                            });
                        }
                        #pragma warning restore CS8601
                    }
                    await _context.SaveChangesAsync();
                }else{
                    throw new NotFoundException(nameof(Track));
                }
            }else{
                throw new NotFoundException(id, nameof(Playlist));
            }
        }


        public async Task RemoveTracksFromPlaylist(int id, List<TrackDTO> lTD)
        {
            Playlist? p = await _context.Playlists.FindAsync(id);
            
            if (p != null){
                // If every genre exists and the track isn't null
                if (lTD.All(tDTO => _context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null) == true && p != null){
                    #pragma warning disable CS8604
                    lTD.ForEach(tDTO => {
                        if (_context.PlaylistTracks.FirstOrDefault(pt => (pt.TrackId == tDTO.TrackId && pt.PlaylistId == id)) != null){
                            _context.PlaylistTracks.Remove(_context.PlaylistTracks.FirstOrDefault(pt => (pt.TrackId == tDTO.TrackId && pt.PlaylistId == id)));
                        }
                    });
                    await _context.SaveChangesAsync();
                    #pragma warning restore CS8604
                }else{
                        throw new NotFoundException(nameof(Track));
                }
            }else{
                throw new NotFoundException(id, nameof(Playlist));
            }
        }

        public async Task<PlaylistDTO> GeneratePlaylist(List<TrackDTO> listTrack, int userId)
        {
            try{
                var rand = new Random();
                if (listTrack.Count == 0){
                    throw new BadRequestException("Please provide at least ten tracks");
                }

                var genrePredicted = await _MLHttpClient.PredictGenre(_mapper.Map<List<TrackMachineLearningDTO>>(listTrack));
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
                var action = await this.PostPlaylist(new PlaylistDTO() {
                    KindId = 1,
                    PlaylistName = "Discovery",
                    UserId = userId
                });
                await this.AddTracksToPlaylist(action.PlaylistId, _mapper.Map<List<TrackDTO>>(_mapper.Map<List<Track>>(listTrackWithGenreRandom)));       
                var endpoint = await _bus.GetSendEndpoint(new Uri(_config["RabbitMQ:Notification"]));
                await endpoint.Send<MessageNotificationQueue>(new MessageNotificationQueue(userId, true));
                return action;
            }catch(Exception e){
                var endpoint = await _bus.GetSendEndpoint(new Uri(_config["RabbitMQ:Notification"]));
                await endpoint.Send<MessageNotificationQueue>(new MessageNotificationQueue(userId, false));
                throw e;      
            }
        }

        public async Task<PlaylistDTO> GeneratePlaylistDev(List<TrackDTO> listTrack, int userId) // TO REMOVE
        {
            try{
                var rand = new Random();
                if (listTrack.Count == 0){
                    throw new BadRequestException("Please provide at least one tracks");
                }

                string genrePredicted = "Electronic";

                var g = _context.Genres.FirstOrDefault(g => g.Name == genrePredicted);
                
                GenreDTO gDTO = _mapper.Map<GenreDTO>(g);
                List<TrackWithGenresDTO> listTrackWithGenre = await _trackService.GetTracksByGenre(gDTO.GenreId);
                List<TrackWithGenresDTO> listTrackWithGenreRandom = listTrackWithGenre.OrderBy(x => rand.Next()).Take(20).ToList();
                var action = await this.PostPlaylist(new PlaylistDTO() {
                    KindId = 1,
                    PlaylistName = "Discovery",
                    UserId = userId
                });
                await this.AddTracksToPlaylist(action.PlaylistId, _mapper.Map<List<TrackDTO>>(_mapper.Map<List<Track>>(listTrackWithGenreRandom)));
                var endpoint = await _bus.GetSendEndpoint(new Uri(_config["RabbitMQ:Notification"]));
                await endpoint.Send<MessageNotificationQueue>(new MessageNotificationQueue(userId, true));
                return action;
            }catch(Exception e){
                var endpoint = await _bus.GetSendEndpoint(new Uri(_config["RabbitMQ:Notification"]));
                await endpoint.Send<MessageNotificationQueue>(new MessageNotificationQueue(userId, false));
                throw e;
            }
        }

        public async Task DeletePlaylists(int userId)
        {
            // TODO : Check user exists ?
            List<PlaylistWithTracksDTO> listPlaylistUser = await this.GetPlaylistsByUserId(userId);
            if (listPlaylistUser != null && listPlaylistUser.Count != 0){
                listPlaylistUser.ForEach(p => {
                    _context.Playlists.Remove(_mapper.Map<Playlist>(p));
                });
                await _context.SaveChangesAsync();                
            }
        }

        public async Task<PlaylistDTO> AddFavoritePlaylist(int userId)
        {
            // TODO : Check user exists ?
            List<PlaylistWithTracksDTO> listPlaylistUser = await this.GetPlaylistsByUserId(userId);
            if (listPlaylistUser != null && listPlaylistUser.Count != 0){
                if (listPlaylistUser.Any(p => p.PlaylistName == "Favorite Musics" && p.KindId == 3)){
                    throw new AlreadyExistsException(userId);
                }   
            }

            PlaylistDTO pDTO = new PlaylistDTO() {
                KindId = 3,
                PlaylistName = "Favorite Musics",
                UserId = userId
            };
            Playlist p = _mapper.Map<Playlist>(pDTO);
            _context.Playlists.Add(p);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlaylistDTO>(p);             
        }

        public async Task<List<PlaylistDTO>> GetPlaylistsWithoutTrackForUser(int trackId, int userId)
        {
            // TODO : Chech user exists ?
            List<PlaylistWithTracksDTO> listP = await this.GetPlaylistsByUserId(userId);
            List<PlaylistWithTracksDTO> listPlaylistsToRemove = new List<PlaylistWithTracksDTO>();

            listP.ForEach(p => {
                if (p.Tracks.Any(t => t.TrackId == trackId)){
                    listPlaylistsToRemove.Add(p);
                }
            });

            listPlaylistsToRemove.ForEach(p =>{
                listP.Remove(p);
            });

            return _mapper.Map<List<PlaylistDTO>>(_mapper.Map<List<Playlist>>(listP));
        }

        public async Task AddTrackToFavoritePlaylist(int userId, TrackDTO track)
        {
            Playlist? p = await _context.Playlists
                .Where(p => p.UserId == userId && p.KindId == 3)
                .FirstOrDefaultAsync();

            if (p != null){
                await this.AddTracksToPlaylist(p.PlaylistId, new List<TrackDTO>(){ track });
            }else{
                throw new NotFoundException(userId);
            }
        }

        public async Task RemoveTrackFromFavoritePlaylist(int userId, TrackDTO track)
        {
            Playlist? p = await _context.Playlists
                .Where(p => p.UserId == userId && p.KindId == 3)
                .FirstOrDefaultAsync();

            if (p != null){
                await this.RemoveTracksFromPlaylist(p.PlaylistId, new List<TrackDTO>(){ track });
            }else{
                throw new NotFoundException(userId);
            }
        }
    }
}
