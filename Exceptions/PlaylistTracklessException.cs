namespace MusicService.Exceptions
{
    [Serializable]
    public class PlaylistTracklessException : Exception
    {
        private static readonly string DefaultMessage = "This playlist was trackless";
        private int PlaylistId { get; }
        public PlaylistTracklessException(int playlistId) : base(DefaultMessage) {
            PlaylistId = playlistId;
        }
        public PlaylistTracklessException(string message) : base(message) {
        }
        
    }
}