namespace MusicService.Models
{
    public class Track
    {
        public int TrackId { get; set; }
        public string? ArtistName { get; set; }
        public string? TrackName { get; set; }
        public double? Popularity { get; set; }
        public double? Acousticness { get; set; }
        public double? Danceability { get; set; }
        public double? DurationMs { get; set; }
        public string? Key { get; set; }
        public double? Tempo { get; set; }
        public double? Energy { get; set; }
        public double? Instrumentalness { get; set;}
        public double? Liveness { get; set; }
        public double? Loudness { get; set; }
        public double? Speechiness { get; set; }
        public double? Valence { get; set; }
        public virtual ICollection<TrackGenre>? TrackGenres { get; set; }
        public virtual ICollection<PlaylistTrack>? PlaylistTracks { get; set; }
    }
}