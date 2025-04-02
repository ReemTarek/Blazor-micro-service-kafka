using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaConsumer(string[] topics)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "ecommerce-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topics);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                await HandleConsume(stoppingToken);
            }, stoppingToken);
        }
        protected async Task HandleConsume(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                await ConsumeAsync(consumeResult);
            }
            _consumer.Close();
        }
        //will be overriden by the derived class
        protected virtual Task ConsumeAsync(ConsumeResult<string,string> consumeResult)
        {
            return Task.CompletedTask;
        }
    }
}
