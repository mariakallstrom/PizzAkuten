﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Dish
    {
        public int DishId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Ingredients")]
        public List<DishIngredient> DishIngredients { get; set; }
        [Display(Name = "Orderrader")]
        public List<OrderDish> OrderDishes { get; set; }
    }
}
