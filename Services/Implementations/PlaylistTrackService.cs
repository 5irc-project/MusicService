using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;

namespace MusicService.Services.Implementations
{
    public class PlaylistTrackService : IPlaylistTrackService
     {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public PlaylistTrackService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<PlaylistTrackDTO>> GetAll()
        {
            return _mapper.Map<List<PlaylistTrackDTO>>(await _context.PlaylistTracks.ToListAsync());
        }

        public async Task AddTracksToPlaylist(int playlistId, List<TrackDTO> lTD)
        {
            Playlist? p = await _context.Playlists.FindAsync(playlistId);
            
            // If every genre exists and the track isn't null
            if (lTD.All(tDTO => _context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null) == true && p != null){
                lTD.ForEach(async tDTO => {
                    Track? t = await _context.Tracks.FindAsync(tDTO.TrackId);
                    #pragma warning disable CS8601
                    _context.PlaylistTracks.Add(new PlaylistTrack {
                        TrackId = tDTO.TrackId,
                        Track = t,
                        PlaylistId = playlistId,
                        Playlist = p
                    });
                    #pragma warning restore CS8601
                });
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveTracksFromPlaylist(int playlistId, List<TrackDTO> lTD)
        {
            Playlist? p = await _context.Playlists.FindAsync(playlistId);
            
            // If every genre exists and the track isn't null
            if (lTD.All(tDTO => _context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null) == true && p != null){
                #pragma warning disable CS8604
                lTD.ForEach(tDTO => {
                    _context.PlaylistTracks.Remove(_context.PlaylistTracks.FirstOrDefault(pt => (pt.TrackId == tDTO.TrackId && pt.PlaylistId == playlistId)));
                });
                await _context.SaveChangesAsync();
                #pragma warning restore CS8604
            }
        }
    }
}