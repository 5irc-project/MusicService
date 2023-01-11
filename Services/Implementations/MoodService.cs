using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MusicService.Exceptions;
using AutoMapper;
using MusicService.DTOs;

namespace MusicService.Services.Implementations
{
    public class MoodService : IMoodService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public MoodService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteMood(int id)
        {
            var mood = await _context.Moods.FindAsync(id);
            if (mood == null)
            {
                throw new MoodNotFoundException(id);
            }
            _context.Moods.Remove(mood);
            await _context.SaveChangesAsync();
        }

        public async Task<MoodDTO?> GetMood(int id)
        {
            return _mapper.Map<MoodDTO>(await _context.Moods.FindAsync(id));
        }

        public async Task<List<MoodDTO>> GetMoods()
        {
            return _mapper.Map<List<MoodDTO>>(await _context.Moods.ToListAsync());
        }

        public async Task PostMood(MoodDTO mDTO)
        {
            _context.Moods.Add(_mapper.Map<Mood>(mDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutMood(int id, MoodDTO mDTO)
        {
            _context.Entry(_mapper.Map<Mood>(mDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Moods.Any(e => e.MoodId == id)){
                    throw new MoodNotFoundException(id);
            }
        }
    }
}