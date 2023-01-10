using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("t_e_mood")]
    public class Mood
    {
        [Column("mood_id")]
        public int MoodId { get; set; }

        [Column("mood_name")]
        public string? Name { get; set; }

        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}