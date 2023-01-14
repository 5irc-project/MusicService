namespace MusicService.DTOs
{
    public class PlaylistDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int KindId { get; set; }
        public string? PlaylistName { get; set; }
    }
}