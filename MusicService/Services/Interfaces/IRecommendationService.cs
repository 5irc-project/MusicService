using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<PlaylistWithTracksDTO> GeneratePlaylist(List<TrackDTO> listTrack);
    }
}