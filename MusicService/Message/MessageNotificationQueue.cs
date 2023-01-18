namespace MusicService.Message
{
    public class MessageNotificationQueue
    {
        public int UserId { get; set; }
        public bool IsGenerated { get; set; }

        public MessageNotificationQueue(int userId, bool isGenerated){
            UserId = userId;
            IsGenerated = isGenerated;
        }
    }
}