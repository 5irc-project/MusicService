namespace MusicService.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public string? Content { get; }
        public string? ReasonPhrase{ get; }
        
        public NotFoundException(int id, string objectType) {
            Content = string.Format("No {0} found with ID = {1}", objectType, id);
            ReasonPhrase = string.Format("{0} Not Found", objectType);
        }

        public NotFoundException(string name, string objectType) {
            Content = string.Format("No {0} found with Name {1}", objectType, name);
            ReasonPhrase = string.Format("{0} Not Found", objectType);
        }

        public NotFoundException(string objectType) {
            Content = string.Format("At least one of the {0} given wasn't found", objectType);
            ReasonPhrase = string.Format("{0} Not Found", objectType);
        }

        public NotFoundException(int userId) {
            Content = string.Format("Favorite playlist for the User with ID = {0} not found", userId);
            ReasonPhrase = "Playlist Not Found";
        }
    }
}