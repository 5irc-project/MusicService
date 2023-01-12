namespace MusicService.Exceptions
{
    [Serializable]
    public class TrackNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Track not found";
        private int TrackId { get; }
        public TrackNotFoundException(int trackId) : base(DefaultMessage) {
            TrackId = trackId;
        }
        public TrackNotFoundException(string message) : base(message) {
        }
    }
}