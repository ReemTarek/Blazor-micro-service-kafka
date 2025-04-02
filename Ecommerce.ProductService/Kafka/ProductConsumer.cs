using Confluent.Kafka;
using Ecommerce.ProductService.Data;
using ECommerce.Common;
using ECommerce.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Ecommerce.ProductService.Kafka
{
    public class ProductConsumer(IServiceProvider serviceProvider, IKafkaProducer kafkaProducer) : KafkaConsumer(topics)
    {
        private static readonly string[] topics = { "order-created" };
        //pass the topics list in consumer constructor
        private ProductDbContext GetDbContext()
        {
            var scope = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        }
        protected override async Task ConsumeAsync(ConsumeResult<string, string> consumeResult)
        {
            await base.ConsumeAsync(consumeResult);
            switch (consumeResult.Topic)
            {
                case "order-created":
                    await HandleOrderCreated(consumeResult.Message.Value);
                    break;
            }
        }
        public async Task HandleOrderCreated(string message)
        {
            if (message != null)
            {
                var orderMessage = JsonConvert.DeserializeObject<OrderModel>(message);
                var isReserveed = await ReserveProduct(orderMessage);
                if (isReserveed)
                {
                    await kafkaProducer.ProduceAsync("product-reserved",orderMessage);
                }
                else
                {
                   await kafkaProducer.ProduceAsync("product-reserve-failed", orderMessage);
                }
            }

        }
        public async Task<bool> ReserveProduct(OrderModel order)
        {
            using var dbcontext = GetDbContext();
            var product = await dbcontext.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
            if (product == null)
            {
                return false;
            }
            else
            {
                if (product.Quantity - order.Quantity<0)
                {
                    return false;
                }
                product.Quantity -= order.Quantity;
                await dbcontext.SaveChangesAsync();
                return true;
            }
        }
    }
}
