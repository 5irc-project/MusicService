using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int KindId { get; set; }
        public Kind Kind { get; set; } = null!;
        public string? PlaylistName { get; set; }
        public virtual ICollection<PlaylistTrack>? PlaylistTracks { get; set; }
    }
}