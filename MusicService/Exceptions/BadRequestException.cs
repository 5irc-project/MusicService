namespace MusicService.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public string? Content { get; }
        public string? ReasonPhrase{ get; }
        
        public BadRequestException(string message) {
            Content = message;
            ReasonPhrase = "Bad Request";
        }
        
    }
}