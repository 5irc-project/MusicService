using System.Text;
using MusicService.Message.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MusicService.Services.Implementations;
using System.Reflection;
using MusicService.DTOs;

namespace MusicService.Message
{
    public class MessageConsumer<T, R>
    {
        private readonly IConnection _conn;
        private readonly object _context;
        public MessageConsumer (object context){
            _context = context;

            var factory = new ConnectionFactory { HostName = "localhost", UserName = "root", Password = "root", AutomaticRecoveryEnabled = true };
            _conn = factory.CreateConnection();
            using (var channel = _conn.CreateModel()){
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (ch, ea) =>
                                {
                                    var body = ea.Body.ToArray();
                                    var message = Encoding.UTF8.GetString(body);
                                    var test = JsonConvert.DeserializeObject<QueueMessage<R>>(message);

                                    var res = typeof(T).GetMethod(test._callback);
                                    var res2 = res.Invoke(context, new object[] { test._obj });
                                };
                channel.BasicConsume(queue: "queue/MusicQueue",
                                 autoAck: true,
                                 consumer: consumer);
            }
        }

        private void mettredansunefction(){
            // mettre dedans a partir du received
        }
    }
}