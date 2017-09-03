using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }

        public string ImagePath { get; set; }

        public bool SpecialDish { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Extra Ingredients")]
        public List<DishExtraIngredient> DishExtraIngredients { get; set; }
        [Display(Name = "Ingredients")]
        public List<DishIngredient> DishIngredients { get; set; }
   
    }
}
