using MassTransit;
using MusicService.Services.Interfaces;

namespace MusicService.Message
{
    public class MessageConsumer : IConsumer<MessageQueue>
    {
        private readonly IPlaylistService _service;

        public MessageConsumer(IPlaylistService service){
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<MessageQueue> context){
            MessageQueue msg = context.Message;
            await _service.GeneratePlaylist(msg.ListTrack, msg.UserId);
        }
    }
}