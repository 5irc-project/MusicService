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
                throw new NotFoundException(id, nameof(Track));
            }
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
        }

        public async Task<TrackWithGenresDTO> GetTrack(int id)
        {
            Track? t = await _context.Tracks
                .Include(t => t.TrackGenres)
                .FirstOrDefaultAsync(t => t.TrackId == id);

            if (t == null){
                throw new NotFoundException(id, nameof(Track));
            }
            
            TrackWithGenresDTO twgDTO = _mapper.Map<TrackWithGenresDTO>(t);
            twgDTO.Genres = new List<GenreDTO>();
            #pragma warning disable CS8602
            foreach(TrackGenre trackGenre in t.TrackGenres){
                twgDTO.Genres.Add(
                    _mapper.Map<GenreDTO>(_context.Genres.Find(trackGenre.GenreId))
                );
            }
            #pragma warning restore CS8602
            return twgDTO;
        }

        public async Task<List<TrackWithGenresDTO>> GetTracks()
        {
            List<Track> lT = await _context.Tracks
                .Include(t => t.TrackGenres)
                .ToListAsync();
            List<TrackWithGenresDTO> ltwgDTO = new List<TrackWithGenresDTO>();
            lT.ForEach(t => {
                TrackWithGenresDTO tGD = _mapper.Map<TrackWithGenresDTO>(t);
                tGD.Genres = new List<GenreDTO>();
                #pragma warning disable CS8602
                foreach(TrackGenre trackGenre in t.TrackGenres){
                    tGD.Genres.Add(
                        _mapper.Map<GenreDTO>(_context.Genres.Find(trackGenre.GenreId))
                    );
                }
                #pragma warning restore CS8602
                ltwgDTO.Add(tGD);
            });
            return ltwgDTO;
        }

        public async Task PostTrack(TrackDTO tDTO)
        {
            if (_context.Tracks.FirstOrDefault(t => t.TrackId == tDTO.TrackId) != null){
                throw new AlreadyExistsException(nameof(Track), tDTO.TrackId);
            }
            
            _context.Tracks.Add(_mapper.Map<Track>(tDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutTrack(int id, TrackDTO tDTO)
        {
            if (!_context.Tracks.Any(e => e.TrackId == id)){
                    throw new NotFoundException(id, nameof(Track));
            }
            _context.Entry(_mapper.Map<Track>(tDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddGenresToTrack(int id, List<GenreDTO> lGD)
        {
            Track? t = await _context.Tracks.FindAsync(id);
            
            // If every genre exists and the track isn't null
            if (t != null){
                if (lGD.All(gDTO => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null) == true){
                    lGD.ForEach(async gDTO => {
                        Genre? g = await _context.Genres.FindAsync(gDTO.GenreId);
                        #pragma warning disable CS8601
                        if (_context.TrackGenres.FirstOrDefault(tg => tg.TrackId == id && tg.GenreId == gDTO.GenreId) == null){
                            _context.TrackGenres.Add(new TrackGenre {
                                GenreId = gDTO.GenreId,
                                Genre = g,
                                TrackId = id,
                                Track = t
                            });
                        }
                        #pragma warning disable CS8601
                    });
                    await _context.SaveChangesAsync();
                }else{
                    throw new NotFoundException(nameof(Genre));
                }
            }else{
                throw new NotFoundException(id, nameof(Track));
            }
        }

        public async Task RemoveGenresFromTrack(int id, List<GenreDTO> lGD)
        {
            Track? t = await _context.Tracks.FindAsync(id);
            
            // If every genre exists and the track isn't null
            if (t != null){
                if (lGD.All(gDTO => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null) == true && t != null){
                    #pragma warning disable 
                    lGD.ForEach(gDTO => {
                        if (_context.TrackGenres.FirstOrDefault(tg => (tg.GenreId == gDTO.GenreId && tg.TrackId == id)) != null){
                            _context.TrackGenres.Remove(_context.TrackGenres.FirstOrDefault(tg => (tg.GenreId == gDTO.GenreId && tg.TrackId == id)));
                        }
                    });
                    await _context.SaveChangesAsync();
                    #pragma warning restore CS8604
                }else{
                        throw new NotFoundException(nameof(Genre));
                }
            }else{
                throw new NotFoundException(id, nameof(Track));
            }
        }

        public async Task PostTrackWithGenres(TrackWithGenresDTO twgDTO)
        {
            if (_context.Tracks.AsNoTracking().FirstOrDefault(t => t.TrackId == twgDTO.TrackId) != null){
                throw new AlreadyExistsException(nameof(Track), twgDTO.TrackId);
            }
            List<GenreDTO> lgDTO = twgDTO.Genres.ToList<GenreDTO>();
            if (lgDTO.All(gDTO => _context.Genres.AsNoTracking().FirstOrDefault(g => g.GenreId == gDTO.GenreId) != null) == true){
                Track t = _mapper.Map<Track>(twgDTO);
                t.TrackGenres = new List<TrackGenre>();
                #pragma warning disable CS8601  
                lgDTO.ForEach(gDTO => {
                    t.TrackGenres.Add(
                        new TrackGenre {
                            Track = t,
                            Genre = _context.Genres.Find(gDTO.GenreId)
                        }
                    );
                });
                #pragma warning restore CS8601  
                _context.Tracks.Add(t);
                await _context.SaveChangesAsync();
            }else{
                throw new NotFoundException(nameof(Genre));
            }
        }
    }
}