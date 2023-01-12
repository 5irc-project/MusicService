using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface ITrackService
    {
        Task<List<TrackDTO>> GetTracks();
        Task<TrackDTO?> GetTrack(int id);
        Task PutTrack(int id, TrackDTO tDTO);
        Task PostTrack(TrackDTO tDTO);
        Task DeleteTrack(int id);
    }
}