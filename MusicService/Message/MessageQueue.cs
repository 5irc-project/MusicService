using MusicService.DTOs;

namespace MusicService.Message
{
    public class MessageQueue
    {
        public List<TrackDTO> ListTrack { get; set; }

        public MessageQueue(List<TrackDTO> listTrack){
            ListTrack = listTrack;
        }
    }
}