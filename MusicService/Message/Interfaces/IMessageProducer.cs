namespace MusicService.Message.Interfaces
{
    public interface IMessageProducer
    {
        void ProduceMessage<T> (T message);
    }
}