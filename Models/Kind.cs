using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("kind")]
    [Index(nameof(Name), IsUnique = true)]
    public class Kind
    {
        [Column("k_id")]
        public int KindId { get; set; }

        [Column("k_name")]
        public string? Name { get; set; }

        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}