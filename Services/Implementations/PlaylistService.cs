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
                throw new PlaylistNotFoundException(id);
            }
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<PlaylistDTO?> GetPlaylist(int id)
        {
            return _mapper.Map<PlaylistDTO>(await _context.Playlists.FindAsync(id));
        }

        public async Task<List<PlaylistDTO>> GetPlaylists()
        {
            return _mapper.Map<List<PlaylistDTO>>(await _context.Playlists.ToListAsync());
        }

        public async Task PostPlaylist(PlaylistDTO pDTO)
        {
            if (pDTO.Tracks?.All(t => _context.Tracks.Find(t.TrackId) != null) == true){
                _context.Playlists.Add(_mapper.Map<Playlist>(pDTO));
                await _context.SaveChangesAsync();
            }else{
                throw new TrackNotFoundException("The tracks given do not exist");
            }
        }

        public async Task PutPlaylist(int id, PlaylistDTO pDTO)
        {
            if (pDTO.Tracks?.All(t => _context.Tracks.Find(t.TrackId) != null) == true){
                _context.Entry(_mapper.Map<Playlist>(pDTO)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                if (!_context.Playlists.Any(e => e.PlaylistId == id)){
                        throw new PlaylistNotFoundException(id);
                }
            }else{
                throw new TrackNotFoundException("The tracks given do not exist");
            }
        }
    }
}