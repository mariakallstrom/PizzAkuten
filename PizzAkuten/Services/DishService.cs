using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            return _context.Ingredients.OrderBy(x=>x.Name).ToList();
        }

        public List<ExtraIngredient> GetAllExtraIngredients()
        {
            return _context.ExtraIngredients.OrderBy(x => x.Name).ToList();
        }

        public List<Dish> GetAllDishesForMenu()
        {
            return _context.Dishes.Where(n=>n.SpecialDish == false).OrderBy(c=>c.CategoryId).Include(x => x.DishIngredients).ThenInclude(i => i.Ingredient).Include(c => c.Category).ToList();
        }
        public List<Dish> GetAllDishes()
        {
            return _context.Dishes.Include(x => x.DishIngredients).ThenInclude(i => i.Ingredient).Include(c => c.Category).ToList();
        }

        public bool CheckIfImageExistsInImageFolder(IFormFile file)
        {
            var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string webRootPath = _hostingEnvironment.WebRootPath;
            var checkImage = webRootPath + "\\images\\" + file.FileName;
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

        public Dish SaveDishToDatabase(IFormCollection form)
        {
            var dish = new Dish();
            var file = form.Files[0];

            dish.ImagePath = GetImagePath(file);

            dish.Name = form["Name"];
            dish.Price = Convert.ToInt32(form["Price"]);

            dish.Category = _context.Categories.SingleOrDefault(x => x.CategoryId == Convert.ToInt32(form["CategoryId"]));


            var allIngredients = _context.Ingredients.ToList();

            var keys = form.Keys.FirstOrDefault(k => k.Contains("Checked-"));
            var dashPosition = keys.IndexOf("-");
            var checkedIngredients = form.Keys.Where(k => k.Contains("Checked-"));

            dish.DishIngredients = new List<DishIngredient>();

            foreach (var ingredient in checkedIngredients)
            {
                var id = int.Parse(ingredient.Substring(dashPosition + 1));
                dish.DishIngredients.Add(new DishIngredient { IngredientId = id, DishId = dish.DishId });
            }

            _context.Add(dish);
            _context.SaveChanges();

            return _context.Dishes.LastOrDefault();
        }

        public Dish EditDish(IFormCollection form)
        {
            var dish = _context.Dishes.FirstOrDefault(x => x.DishId == Convert.ToInt32(form["DishId"]));
            var dishToUpdateDishIngredients = _context.DishIngredients.Where(x => x.DishId == dish.DishId).Select(i=>i.IngredientId).ToList();

            var file = form.Files[0];

            dish.ImagePath = GetImagePath(file);

            dish.Name = form["Name"];
            dish.Price = Convert.ToInt32(form["Price"]);

            dish.Category = _context.Categories.SingleOrDefault(x => x.CategoryId == Convert.ToInt32(form["CategoryId"]));


            var allIngredients = _context.Ingredients.ToList();

            var keys = form.Keys.FirstOrDefault(k => k.Contains("Checked-"));
            var dashPosition = keys.IndexOf("-");
            var checkedIngredients = form.Keys.Where(k => k.Contains("Checked-"));

            var newDishIngredientsList = new List<int>();

            foreach (var ingredient in checkedIngredients)
            {
                var id = int.Parse(ingredient.Substring(dashPosition + 1));
                newDishIngredientsList.Add(id);
            }

            var newIngredients = newDishIngredientsList.Except(dishToUpdateDishIngredients).ToList();
            var oldIngredients = dishToUpdateDishIngredients.Except(newDishIngredientsList).ToList();

            foreach (var number in newIngredients)
            {
                var dishIng = new DishIngredient { Ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == number), Dish = dish };
                _context.DishIngredients.Add(dishIng);
                _context.SaveChanges();
            }

            foreach (var number in oldIngredients)
            {
                var dishIng = _context.DishIngredients.FirstOrDefault(x => x.IngredientId == number);
                _context.DishIngredients.Remove(dishIng);
            }

            dish.Price = Convert.ToInt32(form["Price"]);
            dish.Name = form["Name"];

            dish.Category = _context.Categories.FirstOrDefault(x => x.CategoryId == Convert.ToInt32(form["CategoryId"]));

            _context.Update(dish);
            _context.SaveChanges();
            return dish;

        }

        public string GetImagePath(IFormFile file)
        {

            if (file != null)
            {
                var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                if (!CheckIfImageExistsInImageFolder(file))
                {
                    if (file.Length > 0)
                    {
                        var fileStream = new FileStream(Path.Combine(upload, file.FileName), FileMode.Create);
                        file.CopyToAsync(fileStream);
                    }
                }

               return "/images/" + file.FileName;
            }
            return "Finns inte";
        }

    }
}
