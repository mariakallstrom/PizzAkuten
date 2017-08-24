using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class EditDishViewModel
    {
        public Dish Dish { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
