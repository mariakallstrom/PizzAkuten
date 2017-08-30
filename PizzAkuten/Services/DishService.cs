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

        public async Task<Dish> SaveDishToDatabase(IFormCollection form)
        {
            var dish = new Dish();
            var file = form.Files[0];

            if (file != null)
            {
                var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                if (!CheckIfImageExistsInImageFolder(file))
                {
                    if (file.Length > 0)
                    {
                        var fileStream = new FileStream(Path.Combine(upload, file.FileName), FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }
                }

                dish.ImagePath = "/images/" + file.FileName;
            }
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
            //var dishToUpdate = _context.Dishes.FirstOrDefault(m => m.DishId == dish.DishId);
            //var dishToUpdateDishIngredients = _context.DishIngredients.Where(x => x.DishId == dish.DishId).ToList();

            //foreach (var item in dish.Ingredients)
            //{
            //    if (item.IsChecked == true)
            //    {
            //        //find ingrediens in DishIngredinens
            //        var findIng = dishToUpdateDishIngredients.FirstOrDefault(x => x.IngredientId == item.IngredientId);
            //        if (findIng == null)
            //        {
            //            var ing = _context.DishIngredients.FirstOrDefault(x => x.Ingredient == item);
            //            ing.Dish = dishToUpdate;
            //            ing.Ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == item.IngredientId);
            //            _context.DishIngredients.Add(ing);
            //            _context.SaveChanges();
            //        }

            //    }
            //    if (item.IsChecked == false)
            //    {
            //        var findIng = dishToUpdateDishIngredients.FirstOrDefault(x => x.IngredientId == item.IngredientId);
            //        if (findIng != null)
            //        {
            //            var ing = _context.DishIngredients.FirstOrDefault(x => x.Ingredient == item);
            //            ing.Dish = dishToUpdate;
            //            ing.Ingredient = _context.Ingredients.FirstOrDefault(x => x.IngredientId == item.IngredientId);
            //            _context.DishIngredients.Remove(ing);
            //            _context.SaveChanges();
            //        }

            //    }
            //}

       
            //dishToUpdate.Price = dish.Price;
            //dishToUpdate.Name = dish.Name;
            //dishToUpdate.ImagePath = dish.ImagePath;
            //dishToUpdate.Category = dish.Category;

            //_context.Update(dishToUpdate);
            //_context.SaveChanges();
            return null;
        }

    }
}
