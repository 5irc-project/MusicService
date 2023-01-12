using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("playlist")]
    public class Playlist
    {
        [Column("p_id")]
        public int PlaylistId { get; set; }

        [Column("u_id")]
        public int UserId { get; set; }

        [Column("k_id")]
        [ForeignKey("KindId")]
        public int KindId { get; set; }

        [Column("p_name")]
        public string? PlaylistName { get; set; }

        public virtual ICollection<Track>? Tracks { get; set; }
    }
}