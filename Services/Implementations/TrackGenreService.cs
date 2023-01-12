using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;

namespace MusicService.Services.Implementations
{
    public class TrackGenreService : ITrackGenreService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public TrackGenreService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<TrackGenreDTO>> GetAll()
        {
            return _mapper.Map<List<TrackGenreDTO>>(await _context.TrackGenres.ToListAsync());
        }

        public async Task AddGenresToTrack(int trackId, List<GenreDTO> lGD)
        {
            Track? t = await _context.Tracks.FindAsync(trackId);
            
            // If every genre exists and the track isn't null
            if (lGD.All(gDTO => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null) == true && t != null){
                lGD.ForEach(async gDTO => {
                    Genre? g = await _context.Genres.FindAsync(gDTO.GenreId);
                    #pragma warning disable CS8601
                    _context.TrackGenres.Add(new TrackGenre {
                        GenreId = gDTO.GenreId,
                        Genre = g,
                        TrackId = trackId,
                        Track = t
                    });
                    #pragma warning disable CS8601
                });
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveGenresFromTrack(int trackId, List<GenreDTO> lGD)
        {
            Track? t = await _context.Tracks.FindAsync(trackId);
            
            // If every genre exists and the track isn't null
            if (lGD.All(gDTO => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null) == true && t != null){
                #pragma warning disable CS8604
                lGD.ForEach(gDTO => {
                    _context.TrackGenres.Remove(_context.TrackGenres.FirstOrDefault(tg => (tg.GenreId == gDTO.GenreId && tg.TrackId == trackId)));
                });
                await _context.SaveChangesAsync();
                #pragma warning restore CS8604
            }
        }
    }
}