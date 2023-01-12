using MusicService.Models;

namespace MusicService.DTOs
{
    public class PlaylistGetDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int KindId { get; set; }
        public string? PlaylistName { get; set; }
        public virtual ICollection<TrackDTO> Tracks { get; set; } = null!;
    }
}