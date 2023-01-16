using MusicService.Models;
using MusicService.Exceptions;
using MusicService.DTOs;
using MusicService.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MusicService.Services.Implementations
{
    public class PlaylistService : IPlaylistService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public PlaylistService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            pwtDTO.Tracks = new List<TrackDTO>();
            #pragma warning disable CS8602
            foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                pwtDTO.Tracks.Add(
                    _mapper.Map<TrackDTO>(_context.Tracks.Find(playlistTrack.TrackId))
                );
            }
            #pragma warning restore CS8602  
            return pwtDTO;
        }

        public async Task<List<PlaylistWithTracksDTO>> GetPlaylistsByUser(int userId)
        {
            List<Playlist>? listPlaylist = await _context.Playlists
                .Include(p => p.Kind)
                .Include(p => p.PlaylistTracks)
                .Where(p => p.UserId == userId)
                .ToListAsync();
            List<PlaylistWithTracksDTO> listPlaylistWithTrack = new List<PlaylistWithTracksDTO>();

            listPlaylist.ForEach(p => {
                PlaylistWithTracksDTO pwtDTO = _mapper.Map<PlaylistWithTracksDTO>(p);
                pwtDTO.Tracks = new List<TrackDTO>();
                #pragma warning disable CS8602
                foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                    pwtDTO.Tracks.Add(
                        _mapper.Map<TrackDTO>(_context.Tracks.Find(playlistTrack.TrackId))
                    );
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
                pwtDTO.Tracks = new List<TrackDTO>();
                #pragma warning disable CS8602
                foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                    pwtDTO.Tracks.Add(
                        _mapper.Map<TrackDTO>(_context.Tracks.Find(playlistTrack.TrackId))
                    );
                }
                #pragma warning restore CS8602
                lpwtDTO.Add(pwtDTO);

            });
            return lpwtDTO;        
        }

        public async Task<PlaylistDTO> PostPlaylist(PlaylistDTO pDTO)
        {
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
            Playlist? p = await _context.Playlists.FindAsync(id);
            
            if (p != null){
                // If every genre exists and the track isn't null
                if (lTD.All(tDTO => _context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null) == true && p != null){
                    lTD.ForEach(async tDTO => {
                        Track? t = await _context.Tracks.FindAsync(tDTO.TrackId);
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
                    });
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
    }
}