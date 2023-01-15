using MusicService.Models;

namespace MusicService.DTOs
{
    public class TrackDTO
    {
        public int TrackId { get; set; }
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
    }
}