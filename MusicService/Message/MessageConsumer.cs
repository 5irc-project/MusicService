using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace MusicService.Message
{
    public class MessageConsumer<T, R>
    {
        private readonly IConfiguration _config;
        private readonly object _context;
        public MessageConsumer (object context, IConfiguration config){
            _context = context;
            _config = config;

            var connection = this.CreateConnection();

            if (connection != null){
                try{
                    using (var channel = connection.CreateModel()){
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (sender, args) => { 
                            ParseMessageAndInvokeMethod(channel, args); 
                        };
                        channel.BasicConsume(
                            queue: _config.GetValue<string>("Queue:Name"),
                            autoAck: true,
                            consumer: consumer
                        );
                    }
                }catch{
                    Console.WriteLine("Queue does not yet exist");
                }
            }else{
                // TODO : Notification ?
            }
        }

        private IConnection CreateConnection(){
            try{
                return new ConnectionFactory { 
                    HostName = _config.GetValue<string>("Queue:HostName"), 
                    UserName = _config.GetValue<string>("Queue:UserName"),  
                    Password = _config.GetValue<string>("Queue:Password"),  
                    AutomaticRecoveryEnabled = true 
                }.CreateConnection();
            }catch{
                return null;
            }
        }

        private void ParseMessageAndInvokeMethod(IModel channel, BasicDeliverEventArgs args){
            var queueMessage = JsonConvert.DeserializeObject<QueueMessage<R>>(Encoding.UTF8.GetString(args.Body.ToArray()));
            if (queueMessage != null){
                var method = typeof(T).GetMethod(queueMessage.Callback);
                if (method != null && queueMessage.Obj != null){
                    method.Invoke(_context, new object[] { queueMessage.Obj });
                }
            }
            // TODO : Notification if problem ?
        }
    }
}