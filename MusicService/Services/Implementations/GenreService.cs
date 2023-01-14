using AutoMapper;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MusicService.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public GenreService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                throw new NotFoundException(id, nameof(Genre));
            }
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<GenreDTO> GetGenre(int id)
        {
            return _mapper.Map<GenreDTO>(await _context.Genres.FindAsync(id));
        }

        public async Task<List<GenreDTO>> GetGenres()
        {
            return _mapper.Map<List<GenreDTO>>(await _context.Genres.ToListAsync());
        }

        public async Task PostGenre(GenreDTO gDTO)
        {
            if (_context.Genres.FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null){
                throw new AlreadyExistsException(nameof(Genre), gDTO.Name);
            }

            _context.Genres.Add(_mapper.Map<Genre>(gDTO));
            await _context.SaveChangesAsync();   
        }

        public async Task PutGenre(int id, GenreDTO gDTO)
        {
            _context.Entry(_mapper.Map<Genre>(gDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Genres.Any(e => e.GenreId == id)){
                throw new NotFoundException(id, nameof(Genre));
            }
        }
    }
}