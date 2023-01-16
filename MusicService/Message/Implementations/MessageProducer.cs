using System.Text;
using MusicService.Message.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MusicService.Message.Implementations
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IConnection _conn;
        public MessageProducer (){
            var factory = new ConnectionFactory { HostName = "localhost", UserName = "root", Password = "root", AutomaticRecoveryEnabled = true };
            _conn = factory.CreateConnection();
        }
        
        public void ProduceMessage<T>(T message)
        {
            using (var channel = _conn.CreateModel()){
                channel.QueueDeclare("queue/MusicQueue", true, false, false, null);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: "queue/MusicQueue", body: body);
            }
        }
    }
}