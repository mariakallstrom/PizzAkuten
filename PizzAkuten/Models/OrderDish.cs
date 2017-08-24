using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class OrderDish
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public int TotalPrice { get; set; }
    }
}
