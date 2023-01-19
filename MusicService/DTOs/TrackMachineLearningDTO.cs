namespace MusicService.DTOs
{
    public class TrackMachineLearningDTO
    {
        public decimal? Popularity { get; set; }
        public decimal? Acousticness { get; set; }
        public decimal? Danceability { get; set; }
        public decimal? DurationMs { get; set; }
        public decimal? Tempo { get; set; }
        public decimal? Energy { get; set; }
        public decimal? Instrumentalness { get; set;}
        public decimal? Liveness { get; set; }
        public decimal? Loudness { get; set; }
        public decimal? Speechiness { get; set; }
        public decimal? Valence { get; set; }
    }
}