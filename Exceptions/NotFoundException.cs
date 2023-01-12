namespace MusicService.Exceptions
{
    public class NotFoundException : Exception
    {
        private int Id { get; }
        private string ObjectName { get; }
        public NotFoundException(int id, string objectName) {
            Id = id;
            ObjectName = objectName;
        }
    }
}