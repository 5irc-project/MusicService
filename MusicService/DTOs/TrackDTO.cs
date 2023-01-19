using MusicService.Models;

namespace MusicService.DTOs
{
    #pragma warning disable CS0659
    public class TrackDTO
    {
        public int TrackId { get; set; }
        public string? ArtistName { get; set; }
        public string? TrackName { get; set; }
        public decimal? Popularity { get; set; }
        public decimal? Acousticness { get; set; }
        public decimal? Danceability { get; set; }
        public decimal? DurationMs { get; set; }
        public string? Key { get; set; }
        public decimal? Tempo { get; set; }
        public decimal? Energy { get; set; }
        public decimal? Instrumentalness { get; set;}
        public decimal? Liveness { get; set; }
        public decimal? Loudness { get; set; }
        public decimal? Speechiness { get; set; }
        public decimal? Valence { get; set; }

        #pragma warning disable CS8765
        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                TrackDTO t = (TrackDTO) obj;
                return (TrackId == t.TrackId) && (ArtistName == t.ArtistName) && (TrackName == t.TrackName) && (Popularity == t.Popularity) && (Acousticness == t.Acousticness) && (Danceability == t.Danceability) && (DurationMs == t.DurationMs) && (Key == t.Key) && (Tempo == t.Tempo) && (Energy == t.Energy) && (Instrumentalness == t.Instrumentalness) && (Liveness == t.Liveness) && (Loudness == t.Loudness) && (Speechiness == t.Speechiness) && (Valence == t.Valence);
            }
        }
        #pragma warning restore CS8765
    }
    #pragma warning restore CS0659
}