﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class EditDishViewModel
    {
        public Dish EditDish { get; set; }

        public List<ExtraIngredient> ExtraIngredients { get; set; }
    }
}
