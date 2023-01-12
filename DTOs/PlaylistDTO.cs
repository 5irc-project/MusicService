namespace MusicService.DTOs
{
    public class PlaylistDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int MoodId { get; set; }
        public string? PlaylistName { get; set; }
        public virtual ICollection<TrackDTO>? Tracks { get; set; }
    }
}