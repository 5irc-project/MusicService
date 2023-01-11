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
            _context.Playlists.Add(_mapper.Map<Playlist>(pDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutPlaylist(int id, PlaylistDTO pDTO)
        {
            _context.Entry(_mapper.Map<Playlist>(pDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Playlists.Any(e => e.PlaylistId == id)){
                    throw new PlaylistNotFoundException(id);
            }
        }
    }
}