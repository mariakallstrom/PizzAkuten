using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class ExtraIngredient
    {
        public int ExtraIngredientId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public int Price { get; set; }
        public List<DishExtraIngredient> DishExtraIngredients { get; set; }

    }
}
