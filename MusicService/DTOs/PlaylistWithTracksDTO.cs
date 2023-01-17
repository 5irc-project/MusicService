using MusicService.Models;

namespace MusicService.DTOs
{
    #pragma warning disable CS0659
    public class PlaylistWithTracksDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int KindId { get; set; }
        public string? PlaylistName { get; set; }
        public virtual ICollection<TrackWithGenresDTO> Tracks { get; set; } = null!;

        #pragma warning disable CS8765
        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                PlaylistWithTracksDTO p = (PlaylistWithTracksDTO) obj;
                foreach(var track in p.Tracks){
                    if (!Tracks.Contains(track)){
                        return false;
                    }
                }
                return (PlaylistId == p.PlaylistId) && (UserId == p.UserId) && (KindId == p.KindId) && (PlaylistName == p.PlaylistName);
            }
        }
        #pragma warning restore CS8765
    }
    #pragma warning restore CS0659
}