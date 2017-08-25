using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }

        public bool IsChecked { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
