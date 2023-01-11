using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistDTO>> GetPlaylists();
        Task<PlaylistDTO?> GetPlaylist(int id);
        Task PutPlaylist(int id, PlaylistDTO pDTO);
        Task PostPlaylist(PlaylistDTO pDTO);
        Task DeletePlaylist(int id);
    }
}