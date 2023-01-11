namespace MusicService.Exceptions
{
    [Serializable]
    public class GenreNotFoundException : Exception
    {
        private static readonly string DefaultMessage = "Genre not found";
        private int GenreId { get; }

        public GenreNotFoundException(int genreId) : base(DefaultMessage) {
            GenreId = genreId;
        }
    }
}