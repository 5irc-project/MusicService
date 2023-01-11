using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IMusicService
    {
        Task<List<MusicDTO>> GetMusics();
        Task<MusicDTO?> GetMusic(int id);
        Task PutMusic(int id, MusicDTO mDTO);
        Task PostMusic(MusicDTO mDTO);
        Task DeleteMusic(int id);
    }
}