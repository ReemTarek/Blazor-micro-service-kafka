﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }  
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string ?Status { get; set; }

    }
}
