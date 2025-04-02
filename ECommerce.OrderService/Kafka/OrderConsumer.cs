using Confluent.Kafka;
using ECommerce.Common;
using ECommerce.Model;
using ECommerce.OrderService.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECommerce.OrderService.Kafka
{
    public class OrderConsumer(IServiceProvider serviceProvider) : KafkaConsumer(topics)
    {
        private static readonly string[] topics = { "payment-processed", "product-reserve-failed", "product-reservation-cancel" };

        private OrderDbContext GetDbContext()
        {
            var scope = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        }
        protected override async Task ConsumeAsync(ConsumeResult<string, string> consumeResult)
        {
            await base.ConsumeAsync(consumeResult);
            switch (consumeResult.Topic)
            {
                case "payment-processed":
                    await HandleOrderConfirmed(consumeResult.Message.Value);
                    break;
                case "product-reserve-failed":
                    await HandleCancelOrder(consumeResult.Message.Value);
                    break;
                case "product-reservation-cancel":
                    await HandleCancelOrder(consumeResult.Message.Value);
                    break;
            }
        }


        public async Task HandleOrderConfirmed(string message)
        {
            var ordermessage = JsonConvert.DeserializeObject<OrderModel>(message);
            if (ordermessage != null)
            {
                using var dbContext = GetDbContext();
                var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == ordermessage.Id);
                if (order != null)
                {
                    order.Status = "Confirmed";
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        public async Task HandleCancelOrder(string message)
        {
            var ordermessage = JsonConvert.DeserializeObject<OrderModel>(message);

            if (ordermessage != null)
            {
                using var dbContext = GetDbContext();
                var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == ordermessage.Id);
                if (order != null)
                {
                    order.Status = "Cancelled";
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
