using Confluent.Kafka;
using ECommerce.Common;
using ECommerce.Model;
using Newtonsoft.Json;

namespace ECommerce.PaymentService.Kafka
{
    public class PaymentConsumer(IKafkaProducer kafkaProducer) : KafkaConsumer(topics)
    {
        private static readonly string[] topics = { "product-reserved" };

        protected override async Task ConsumeAsync(ConsumeResult<string, string> consumeResult)
        {
            await base.ConsumeAsync(consumeResult);
            switch (consumeResult.Topic)
            {
                case "product-reserved":
                    await HandleProductReserved(consumeResult.Message.Value);
                    break;
            }
        }
        public async Task HandleProductReserved(string message)
        {
            if (message != null)
            {
                var orderMessage = JsonConvert.DeserializeObject<OrderModel>(message);
                var isPaymentSuccess =  ProcessPayment(orderMessage);
                if (isPaymentSuccess)
                {
                    await kafkaProducer.ProduceAsync("payment-processed", orderMessage);
                }
                else
                {
                    await kafkaProducer.ProduceAsync("payment-failed", orderMessage);
                }
            }
        }
        public  bool ProcessPayment(OrderModel orderMessage)
        {
            //payment processing logic
            return false;
        }
    }
}
