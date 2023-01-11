using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IMusicService
    {
        Task<List<MusicDTO>> GetMusics();
        Task<MusicDTO?> GetMusic(int id);
        Task PutMusic(int id, MusicDTO music);
        Task PostMusic(MusicDTO music);
        Task DeleteMusic(int id);
    }
}