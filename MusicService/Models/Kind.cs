namespace MusicService.Models
{

    public class Kind
    {
        public int KindId { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}