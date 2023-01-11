using MusicService.Models;

namespace MusicService.DTOs
{
    public class MusicDTO
    {
        public int MusicId { get; set; }
        public string? ArtistName { get; set; }
        public string? TrackName { get; set; }
        public float? Popularity { get; set; }
        public float? Acousticness { get; set; }
        public float? Danceability { get; set; }
        public float? DurationMs { get; set; }
        public string? Key { get; set; }
        public float? Tempo { get; set; }
        public float? Energy { get; set; }
        public float? Instrumentalness { get; set;}
        public float? Liveness { get; set; }
        public float? Loudness { get; set; }
        public float? Speechiness { get; set; }
        public float? Valence { get; set; }
        public virtual ICollection<Genre>? Genres { get; set; }
    }
}