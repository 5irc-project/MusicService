using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MusicService.Exceptions;
using AutoMapper;
using MusicService.DTOs;

namespace MusicService.Services.Implementations
{
    public class KindService : IKindService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public KindService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteKind(int id)
        {
            var kind = await _context.Kinds.FindAsync(id);
            if (kind == null)
            {
                throw new KindNotFoundException(id);
            }
            _context.Kinds.Remove(kind);
            await _context.SaveChangesAsync();
        }

        public async Task<KindDTO?> GetKind(int id)
        {
            return _mapper.Map<KindDTO>(await _context.Kinds.FindAsync(id));
        }

        public async Task<List<KindDTO>> GetKinds()
        {
            return _mapper.Map<List<KindDTO>>(await _context.Kinds.ToListAsync());
        }

        public async Task PostKind(KindDTO kDTO)
        {
            _context.Kinds.Add(_mapper.Map<Kind>(kDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutKind(int id, KindDTO kDTO)
        {
            _context.Entry(_mapper.Map<Kind>(kDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Kinds.Any(e => e.KindId == id)){
                    throw new KindNotFoundException(id);
            }
        }
    }
}