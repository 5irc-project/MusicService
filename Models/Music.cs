using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("t_e_music_mus")]
    public class Music
    {
        [Column("mus_id")]
        public int MusicId { get; set; }

        [Column("mus_artist_name")]
        public string? ArtistName { get; set; }

        [Column("mus_track_name")]
        public string? TrackName { get; set; }

        [Column("mus_popularity")]
        [Range(0, 100)]
        public float? Popularity { get; set; }

        [Column("mus_acousticness")]
        [Range(0, 1)]
        public float? Acousticness { get; set; }

        [Column("mus_danceability")]
        [Range(0, 1)]
        public float? Danceability { get; set; }

        [Column("mus_duration_ms")]
        [Range(0, float.MaxValue)]
        public float? DurationMs { get; set; }

        [Column("mus_key")]
        public string? Key { get; set; }

        [Column("mus_tempo")]
        [Range(0, float.MaxValue)]
        public float? Tempo { get; set; }

        [Column("mus_energy")]
        [Range(0, 1)]
        public float? Energy { get; set; }

        [Column("mus_instrumentalness")]
        [Range(0, 1)]
        public float? Instrumentalness { get; set;}

        [Column("mus_liveness")]
        [Range(0, 1)]
        public float? Liveness { get; set; }

        [Column("mus_loudness")]
        [Range(-60, 10)]
        public float? Loudness { get; set; }

        [Column("mus_speechiness")]
        [Range(0, 1)]
        public float? Speechiness { get; set; }

        [Column("mus_valence")]
        [Range(0, 1)]
        public float? Valence { get; set; }

        public virtual ICollection<Genre>? Genres { get; set; }

        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}