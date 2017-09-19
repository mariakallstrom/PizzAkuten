using PizzAkuten.Data;
using PizzAkuten.Models;
using System.Collections.Generic;
using System.Linq;

namespace PizzAkuten.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
