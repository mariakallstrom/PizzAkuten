using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PizzAkuten.Data;
using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Services
{
    public class DishService
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DishService(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _context.Ingredients.ToList();
        }

        public bool CheckIfImageExistsInImageFolder(IFormFile file)
        {
            var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string webRootPath = _hostingEnvironment.WebRootPath;
            var checkImage = webRootPath + "//images//" + file.FileName;
            string[] filesInDirectoryImages = Directory.GetFiles(upload);
            foreach (var fileName in filesInDirectoryImages)
            {
                if (fileName == checkImage)
                {
                    return true;
                }

            }
            return false;
        }

        public Dish SaveDishToDatabase(AdminDishViewModel dish)
        {
            var newDish = new Dish();
           
            var dishIngredients = new List<DishIngredient>();

            newDish.Category = _context.Categories.SingleOrDefault(x => x.CategoryId == dish.CategoryId);
            newDish.ImagePath = dish.ImagePath;
            newDish.Name = dish.Name;
            newDish.Price = dish.Price;

            foreach (var item in dish.Ingredients)
            {
                if(item.IsChecked)
                {
                    var di = new DishIngredient();
                    di.Ingredient = item;
                    di.IngredientId = item.IngredientId;
                    dishIngredients.Add(di);
                }
            }
            newDish.DishIngredients = dishIngredients;


            _context.Add(newDish);
            _context.SaveChangesAsync();

            return newDish;
        }
    }
}
