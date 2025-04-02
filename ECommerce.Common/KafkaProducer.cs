using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message);
    }
    public class KafkaProducer :IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        public  KafkaProducer() { 
            
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        public async Task ProduceAsync<T>(string topic, T message)
        {
            await _producer.ProduceAsync(topic, new Message<string, string> { Value = System.Text.Json.JsonSerializer.Serialize(message) });
        }

    }

}
