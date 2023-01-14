namespace MusicService.Exceptions
{
    [Serializable]
    public class AlreadyExistsException : Exception
    {
        public string? Content { get; }
        public string? ReasonPhrase{ get; }

        public AlreadyExistsException(string objectType, string objectName) {
            Content = string.Format("{0} with Name {1} already exists", objectType, objectName);
            ReasonPhrase = string.Format("{0} Already Exists", objectType);
        }

        public AlreadyExistsException(string objectType, int id) {
            Content = string.Format("{0} with ID = {1} already exists", objectType, id);
            ReasonPhrase = string.Format("{0} Already Exists", objectType);
        }
    }
}