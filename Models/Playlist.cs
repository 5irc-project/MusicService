using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("t_e_playlist_plst")]
    public class Playlist
    {
        [Column("plst_id")]
        public int PlaylistId { get; set; }

        [Column("usr_id")]
        public int UserId { get; set; }

        [Column("mood_id")]
        public int MoodId { get; set; }

        [Column("plst_name")]
        public string? PlaylistName { get; set; }

        public virtual ICollection<Music>? Musics { get; set; }
    }
}