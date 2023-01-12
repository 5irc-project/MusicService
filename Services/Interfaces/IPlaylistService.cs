using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistGetDTO>> GetPlaylists();
        Task<PlaylistGetDTO?> GetPlaylist(int id);
        Task PutPlaylist(int id, PlaylistDTO ppDTO);
        Task PostPlaylist(PlaylistDTO pDTO);
        Task DeletePlaylist(int id);
    }
}