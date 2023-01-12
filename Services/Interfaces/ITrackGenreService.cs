using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface ITrackGenreService
    {
        Task<List<TrackGenreDTO>> GetAll();
        Task AddGenresToTrack(int trackId, List<GenreDTO> lGD);
        Task RemoveGenresFromTrack(int trackId, List<GenreDTO> lGD);
    }
}