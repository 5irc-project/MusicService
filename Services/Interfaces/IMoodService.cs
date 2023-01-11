using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IMoodService
    {
        Task<List<MoodDTO>> GetMoods();
        Task<MoodDTO?> GetMood(int id);
        Task PutMood(int id, MoodDTO mDTO);
        Task PostMood(MoodDTO mDTO);
        Task DeleteMood(int id);
    }
}