using System.Text;
using MusicService.Message.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MusicService.Message.Implementations
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IConfiguration _config;
        private readonly IConnection _conn;
        public MessageProducer (IConfiguration config){
            _config = config;
            _conn = this.CreateConnection();
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
        
        public void ProduceMessage<T>(T message)
        {
            if (_conn != null){
                using (var channel = _conn.CreateModel()){
                    channel.QueueDeclare(_config.GetValue<string>("Queue:Name"), true, false, false, null);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: "", routingKey: _config.GetValue<string>("Queue:Name"), body: body);
                }
            }
        }
    }
}