using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface ITrackService
    {
        Task<List<TrackWithGenresDTO>> GetTracks();
        Task<TrackWithGenresDTO> GetTrack(int id);
        Task<List<TrackWithGenresDTO>> GetTracksByNameQuery(string nameQuery);
        Task PutTrack(int id, TrackDTO tDTO);
        Task PostTrack(TrackDTO tDTO);
        Task PostTrackWithGenres(TrackWithGenresDTO twgDTO);
        Task DeleteTrack(int id);
        Task AddGenresToTrack(int id, List<GenreDTO> lGD);
        Task RemoveGenresFromTrack(int id, List<GenreDTO> lGD);
    }
}