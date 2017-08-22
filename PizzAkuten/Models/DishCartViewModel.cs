using PizzAkuten.Models.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class DishCartViewModel
    {
        public List<Dish> Dish { get; set; }
        public List<CartItemModel> CartItemModel { get; set; }
    }
}
