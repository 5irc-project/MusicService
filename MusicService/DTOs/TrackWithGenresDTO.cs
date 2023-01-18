using MusicService.Models;

namespace MusicService.DTOs
{
    #pragma warning disable CS0659
    public class TrackWithGenresDTO
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
        public virtual ICollection<GenreDTO> Genres { get; set; } = null!; 

        #pragma warning disable CS8765
        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                TrackWithGenresDTO t = (TrackWithGenresDTO) obj;
                foreach(var genre in t.Genres){
                    if (!Genres.Contains(genre)){
                        return false;
                    }
                }
                return (TrackId == t.TrackId) && (ArtistName == t.ArtistName) && (TrackName == t.TrackName) && (Popularity == t.Popularity) && (Acousticness == t.Acousticness) && (Danceability == t.Danceability) && (DurationMs == t.DurationMs) && (Key == t.Key) && (Tempo == t.Tempo) && (Energy == t.Energy) && (Instrumentalness == t.Instrumentalness) && (Liveness == t.Liveness) && (Loudness == t.Loudness) && (Speechiness == t.Speechiness) && (Valence == t.Valence);
            }
        }
        #pragma warning restore CS8765  
    }
    #pragma warning restore CS0659
}