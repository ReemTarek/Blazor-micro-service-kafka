﻿using Ecommerce.ProductService.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<List<ProductModel>> GetProducts()
        {
         return await dbContext.Products.ToListAsync();
             
        }
        [HttpGet("{id}")]
        public async Task<ProductModel> GetProduct(int id)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
