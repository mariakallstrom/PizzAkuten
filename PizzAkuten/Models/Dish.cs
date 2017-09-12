using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        [DisplayName("Namn")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Pris")]
        public int Price { get; set; }
        [DisplayName("Bild")]
        public string ImagePath { get; set; }
        [DisplayName("Specialrätt")]
        public bool SpecialDish { get; set; }

        public int? CategoryId { get; set; }
        [DisplayName("Kategori")]
        public Category Category { get; set; }

        [Display(Name = "Extra ingredienser")]
        public List<DishExtraIngredient> DishExtraIngredients { get; set; }
        [Display(Name = "Ingredienser")]
        public List<DishIngredient> DishIngredients { get; set; }
   
    }
}
