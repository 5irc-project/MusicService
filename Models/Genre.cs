using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("genre")]
    [Index(nameof(Name), IsUnique = true)]
    public class Genre
    {
        [Column("g_id")]
        public int GenreId { get; set; }

        [Column("g_name")]
        public string? Name { get; set; }

        public virtual ICollection<Track>? Tracks { get; set; }
    }
}