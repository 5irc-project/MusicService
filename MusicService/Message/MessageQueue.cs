using MusicService.DTOs;

namespace MusicService.Message
{
    public class MessageQueue
    {
        public List<TrackDTO> ListTrack { get; set; }
        public int UserId { get; set; }

        public MessageQueue(List<TrackDTO> listTrack, int userId){
            ListTrack = listTrack;
            UserId = userId;
        }
    }
}