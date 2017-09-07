﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Kategori")]
        public string Name { get; set; }

        public List<Dish> Dishes { get; set; }
    }
}
