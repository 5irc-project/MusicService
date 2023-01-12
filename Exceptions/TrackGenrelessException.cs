namespace MusicService.Exceptions
{
    [Serializable]
    public class TrackGenrelessException : Exception
    {
        private static readonly string DefaultMessage = "This track was genreless";
        private int TrackId { get; }
        public TrackGenrelessException(int trackId) : base(DefaultMessage) {
            TrackId = trackId;
        }
        public TrackGenrelessException(string message) : base(message) {
        }
    }
}