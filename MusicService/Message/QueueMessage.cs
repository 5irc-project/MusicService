using MusicService.DTOs;

namespace MusicService.Message
{
    public class QueueMessage
    {
        public List<TrackDTO> _obj { get; set; }
        public string _callback { get; set; }

        public QueueMessage(List<TrackDTO> obj, string callback){
            _obj = obj;
            _callback = callback;
        }
    }
}