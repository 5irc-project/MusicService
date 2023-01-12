using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IPlaylistTrackService
    { 
        Task<List<PlaylistTrackDTO>> GetAll();
        Task AddTracksToPlaylist(int playlistId, List<TrackDTO> lTD);
        Task RemoveTracksFromPlaylist(int playlistId, List<TrackDTO> lTD);
    }
}