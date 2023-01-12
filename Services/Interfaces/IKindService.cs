using MusicService.DTOs;

namespace MusicService.Services.Interfaces
{
    public interface IKindService
    {
        Task<List<KindDTO>> GetKinds();
        Task<KindDTO?> GetKind(int id);
        Task PutKind(int id, KindDTO kDTO);
        Task PostKind(KindDTO kDTO);
        Task DeleteKind(int id);
    }
}