using PizzAkuten.Data;
using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Services
{
    public class DishService
    {

        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _context.Ingredients.ToList();
        }
    }
}
