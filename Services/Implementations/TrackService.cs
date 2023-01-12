using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MusicService.DTOs;
using MusicService.Exceptions;
using MusicService.Models;
using MusicService.Services.Interfaces;

namespace MusicService.Services.Implementations
{
    public class TrackService : ITrackService
    {
        private readonly MusicServiceDBContext _context;
        private readonly IMapper _mapper;

        public TrackService(MusicServiceDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                throw new TrackNotFoundException(id);
            }
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
        }

        public async Task<TrackDTO?> GetTrack(int id)
        {
            return _mapper.Map<TrackDTO>(await _context.Tracks.FindAsync(id));
        }

        public async Task<List<TrackDTO>> GetTracks()
        {
            return _mapper.Map<List<TrackDTO>>(await _context.Tracks.ToListAsync());
        }

        public async Task PostTrack(TrackDTO tDTO)
        {
            if (tDTO.Genres?.All(g => _context.Genres.AsNoTracking().FirstOrDefault(gBis => gBis.GenreId == g.GenreId) != null) == true){
                Track t = _mapper.Map<Track>(tDTO);
                _context.Tracks.Add(t);
                foreach(Genre g in t.Genres){
                    _context.Entry(t.Genres.FirstOrDefault(gBis => gBis.GenreId == g.GenreId)).State = EntityState.Unchanged;
                }
                await _context.SaveChangesAsync();
            }else{
                throw new GenreNotFoundException("The given genres do not exist");
            }
            
        }

        public async Task PutTrack(int id, TrackDTO tDTO)
        {
            if (tDTO.Genres?.All(g => _context.Genres.AsNoTracking().FirstOrDefault(gBis => gBis.GenreId == g.GenreId) != null) == true){
                Track t = _mapper.Map<Track>(tDTO);
                _context.Entry(t).State = EntityState.Modified;
               
                await _context.SaveChangesAsync();
                if (!_context.Tracks.Any(e => e.TrackId == id)){
                        throw new TrackNotFoundException(id);
                }
            }else{
                throw new GenreNotFoundException("The given genres do not exist");
            }
        }
    }
}