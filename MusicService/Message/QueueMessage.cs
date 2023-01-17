using MusicService.DTOs;

namespace MusicService.Message
{
    public class QueueMessage<T>
    {
        public T Obj { get; set; }
        public string Callback { get; set; }

        public QueueMessage(T obj, string callback){
            Obj = obj;
            Callback = callback;
        }
    }
}