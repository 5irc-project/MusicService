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

        public async Task<PlaylistGetDTO?> GetPlaylist(int id)
        {
            Playlist? p = await _context.Playlists.FindAsync(id);
            if (p == null){
                return null;
            }
            PlaylistGetDTO pGD = _mapper.Map<PlaylistGetDTO>(p);
            #pragma warning disable CS8604
            foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                pGD.Tracks.Add(
                    _mapper.Map<TrackDTO>(_context.Tracks.Find(playlistTrack.TrackId))
                );
            }
            #pragma warning restore CS8601  
            return pGD;
        }

        public async Task<List<PlaylistGetDTO>> GetPlaylists()
        {
            List<Playlist> lP = await _context.Playlists.ToListAsync();
            List<PlaylistGetDTO> lPG = new List<PlaylistGetDTO>();
            lP.ForEach(p => {
                PlaylistGetDTO pGD = _mapper.Map<PlaylistGetDTO>(p);
                #pragma warning disable CS8604
                foreach(PlaylistTrack playlistTrack in p.PlaylistTracks){
                    pGD.Tracks.Add(
                        _mapper.Map<TrackDTO>(_context.Tracks.Find(playlistTrack.TrackId))
                    );
                }
                #pragma warning restore CS8601 
                lPG.Add(pGD);

            });
            return lPG;        
        }

        public async Task PostPlaylist(PlaylistDTO pDTO)
        {
            _context.Playlists.Add(_mapper.Map<Playlist>(pDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutPlaylist(int id, PlaylistDTO pDTO)
        {
            _context.Entry(_mapper.Map<Playlist>(pDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Playlists.Any(e => e.PlaylistId == id)){
                    throw new NotFoundException(id, nameof(Playlist));
            }
        }
    }
}