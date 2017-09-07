using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        [DisplayName("Namn")]
        public string Name { get; set; }
        [DisplayName("Är vald")]
        public bool IsChecked { get; set; }
        [DisplayName("Pris")]
        public int Price { get; set; }
        [DisplayName("Ingredienser")]
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
