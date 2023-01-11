using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MusicService.Exceptions;
using AutoMapper;
using MusicService.DTOs;

namespace MusicService.Services
{
    public class MusicService : IMusicService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public MusicService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteMusic(int id)
        {
            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                throw new MusicNotFoundException(id);
            }
            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();
        }

        public async Task<MusicDTO?> GetMusic(int id)
        {
            return _mapper.Map<MusicDTO>(await _context.Musics.FindAsync(id));
        }

        public async Task<List<MusicDTO>> GetMusics()
        {
            return _mapper.Map<List<MusicDTO>>(await _context.Musics.ToListAsync());
        }

        public async Task PostMusic(MusicDTO mDTO)
        {
            _context.Musics.Add(_mapper.Map<Music>(mDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutMusic(int id, MusicDTO mDTO)
        {
            _context.Entry(_mapper.Map<Music>(mDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Musics.Any(e => e.MusicId == id)){
                    throw new MusicNotFoundException(id);
            }
        }
    }
}