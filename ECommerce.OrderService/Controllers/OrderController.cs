using Confluent.Kafka;
using ECommerce.Common;
using ECommerce.Model;
using ECommerce.OrderService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerce.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderDbContext dbContext, IKafkaProducer producer) : ControllerBase
    {
        [HttpGet]
        public async Task<List<OrderModel>> GetOrders()
        {
            return await dbContext.Orders.ToListAsync();
        }
        [HttpPost]
        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            //need kafka for message passing
            await producer.ProduceAsync("order-created",order);
            return order;
        }
    }
}
