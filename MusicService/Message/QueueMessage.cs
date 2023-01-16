using MusicService.DTOs;

namespace MusicService.Message
{
    public class QueueMessage<T>
    {
        public T _obj { get; set; }
        public string _callback { get; set; }

        public QueueMessage(T obj, string callback){
            _obj = obj;
            _callback = callback;
        }
    }
}