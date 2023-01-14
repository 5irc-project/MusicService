using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistWithTracksDTO>> GetPlaylists();
        Task<PlaylistWithTracksDTO?> GetPlaylist(int id);
        Task PutPlaylist(int id, PlaylistDTO ppDTO);
        Task PostPlaylist(PlaylistDTO pDTO);
        Task DeletePlaylist(int id);
        Task AddTracksToPlaylist(int id, List<TrackDTO> lTD);
        Task RemoveTracksFromPlaylist(int id, List<TrackDTO> lTD);
    }
}