using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    [Table("track")]
    public class Track
    {
        [Column("t_id")]
        public int TrackId { get; set; }

        [Column("t_artist_name")]
        public string? ArtistName { get; set; }

        [Column("t_track_name")]
        public string? TrackName { get; set; }

        [Column("t_popularity")]
        [Range(0, 100)]
        public float? Popularity { get; set; }

        [Column("t_acousticness")]
        [Range(0, 1)]
        public float? Acousticness { get; set; }

        [Column("t_danceability")]
        [Range(0, 1)]
        public float? Danceability { get; set; }

        [Column("t_duration_ms")]
        [Range(0, float.MaxValue)]
        public float? DurationMs { get; set; }

        [Column("t_key")]
        public string? Key { get; set; }

        [Column("t_tempo")]
        [Range(0, float.MaxValue)]
        public float? Tempo { get; set; }

        [Column("t_energy")]
        [Range(0, 1)]
        public float? Energy { get; set; }

        [Column("t_instrumentalness")]
        [Range(0, 1)]
        public float? Instrumentalness { get; set;}

        [Column("t_liveness")]
        [Range(0, 1)]
        public float? Liveness { get; set; }

        [Column("t_loudness")]
        [Range(-60, 10)]
        public float? Loudness { get; set; }

        [Column("t_speechiness")]
        [Range(0, 1)]
        public float? Speechiness { get; set; }

        [Column("t_valence")]
        [Range(0, 1)]
        public float? Valence { get; set; }

        public virtual ICollection<Genre>? Genres { get; set; }
        
        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}