using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task DispatchRequestToQueue(List<TrackDTO> listTrack, string nameOfMethod);
        Task GeneratePlaylist(List<TrackDTO> listTrack);
    }
}