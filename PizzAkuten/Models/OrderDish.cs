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
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
    }
}
