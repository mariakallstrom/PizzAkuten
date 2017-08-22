using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models.OrderViewModels
{
    public class CartItemModel
    {
        public int Quantity { get; set; }

        public  Dish Dish { get; set; }
    }
}
