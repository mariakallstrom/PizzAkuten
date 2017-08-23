using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Dish Dish { get; set; }

        public int Quantity { get; set; }
    }
}
