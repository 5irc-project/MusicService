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

        public async Task<TrackGetDTO?> GetTrack(int id)
        {
            Track? t = await _context.Tracks.FindAsync(id);
            if (t == null){
                return null;
            }
            TrackGetDTO tGD = _mapper.Map<TrackGetDTO>(t);
            #pragma warning disable CS8604
            foreach(TrackGenre trackGenre in t.TrackGenres){
                tGD.Genres.Add(
                    _mapper.Map<GenreDTO>(_context.Genres.Find(trackGenre.GenreId))
                );
            }
            #pragma warning restore CS8601  
            return tGD;
        }

        public async Task<List<TrackGetDTO>> GetTracks()
        {
            List<Track> lT = await _context.Tracks.ToListAsync();
            List<TrackGetDTO> lTG = new List<TrackGetDTO>();
            lT.ForEach(t => {
                TrackGetDTO tGD = _mapper.Map<TrackGetDTO>(t);
                #pragma warning disable CS8604
                foreach(TrackGenre trackGenre in t.TrackGenres){
                    tGD.Genres.Add(
                        _mapper.Map<GenreDTO>(_context.Genres.Find(trackGenre.GenreId))
                    );
                }
                #pragma warning restore CS8601
                lTG.Add(tGD);
            });
            return lTG;
        }

        public async Task PostTrack(TrackDTO tDTO)
        {
            _context.Tracks.Add(_mapper.Map<Track>(tDTO));
            await _context.SaveChangesAsync();
        }

        public async Task PutTrack(int id, TrackDTO tDTO)
        {
            _context.Entry(_mapper.Map<Track>(tDTO)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!_context.Tracks.Any(e => e.TrackId == id)){
                    throw new NotFoundException(id, nameof(Track));
            }
        }
    }
}