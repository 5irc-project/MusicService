namespace MusicService.DTOs
{
    public class PlaylistDTO
    {
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
        public int KindId { get; set; }
        public string? PlaylistName { get; set; }

        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                PlaylistDTO p = (PlaylistDTO) obj;
                return (PlaylistId == p.PlaylistId) && (UserId == p.UserId) && (KindId == p.KindId) && (PlaylistName == p.PlaylistName);
            }
        }
    }
}