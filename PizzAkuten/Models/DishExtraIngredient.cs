using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class DishExtraIngredient
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int ExtraIngredientId { get; set; }
        public ExtraIngredient ExtraIngredient { get; set; }
    }
}
