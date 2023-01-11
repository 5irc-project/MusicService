using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreDTO>> GetGenres();
        Task<GenreDTO> GetGenre(int id);
        Task PutGenre(int id, GenreDTO gDTO);
        Task PostGenre(GenreDTO gDTO);
        Task DeleteGenre(int id);
    }
}