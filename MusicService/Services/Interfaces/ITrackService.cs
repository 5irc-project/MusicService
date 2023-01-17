using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface ITrackService
    {
        Task<List<TrackWithGenresDTO>> GetTracks();
        Task<TrackWithGenresDTO> GetTrack(int id);
        Task<TrackWithGenresDTO> GetRandomTrack();
        Task<List<TrackWithGenresDTO>> GetTracksByNameQuery(string nameQuery);
        Task<List<TrackWithGenresDTO>> GetTracksByGenre(int genreId);
        Task PutTrack(int id, TrackDTO tDTO);
        Task<TrackDTO> PostTrack(TrackDTO tDTO);
        Task<TrackDTO> PostTrackWithGenres(TrackWithGenresDTO twgDTO);
        Task DeleteTrack(int id);
        Task AddGenresToTrack(int id, List<GenreDTO> lGD);
        Task RemoveGenresFromTrack(int id, List<GenreDTO> lGD);
        Task<List<PlaylistDTO>> GetTrackPlaylists(int id);
        Task<bool> IsTrackInPlaylist(int id, int playlistId);
        Task<bool> IsTrackInFavorite(int id, int userId);
    }
}