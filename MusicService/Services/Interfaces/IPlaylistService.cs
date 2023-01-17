using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistWithTracksDTO>> GetPlaylists();
        Task<PlaylistWithTracksDTO> GetPlaylist(int id);
        Task<List<PlaylistWithTracksDTO>> GetPlaylistsByUserId(int userId);
        Task PutPlaylist(int id, PlaylistDTO ppDTO);
        Task<PlaylistDTO> PostPlaylist(PlaylistDTO pDTO);
        Task DeletePlaylist(int id);
        Task AddTracksToPlaylist(int id, List<TrackDTO> lTD);
        Task RemoveTracksFromPlaylist(int id, List<TrackDTO> lTD);
        Task DeletePlaylists(int userId);
        Task GeneratePlaylist(List<TrackDTO> listTrack);
        Task<PlaylistDTO> AddFavoritePlaylist(int userId);
        Task<List<PlaylistDTO>> GetPlaylistsWithoutTrackForUser(int trackId, int userId);
    }
}