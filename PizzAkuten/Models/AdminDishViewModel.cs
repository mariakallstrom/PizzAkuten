using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class AdminDishViewModel
    {
        public int DishId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string ImagePath { get; set; }

        public IFormFile ImageFile { get; set; }

        public bool SpecialDish { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<SelectListItem> SelectCategoryList { get; set; }

        [Display(Name = "Ingredients")]
        public List<Ingredient> Ingredients { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }

    }
}
