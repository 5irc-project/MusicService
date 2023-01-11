namespace MusicService.Exceptions
{
    [Serializable]
    public class GenreNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Music not found";
        private int GenreId { get; }

        public GenreNotFoundException(int genreId) : base(DefaultMessage) {
            GenreId = genreId;
        }
    }
}