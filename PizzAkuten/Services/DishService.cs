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
            var allIngredients = _context.Ingredients.ToList();

            newDish.Category = _context.Categories.SingleOrDefault(x => x.CategoryId == dish.CategoryId);
            
            newDish.ImagePath = dish.ImagePath;
            newDish.Name = dish.Name;
            newDish.Price = dish.Price;

            foreach (var item in dish.Ingredients)
            {
                if (item.IsChecked)
                {
                    foreach (var i in allIngredients)
                    {
                        if(item.IngredientId == i.IngredientId)
                        {
                            var dishing = new DishIngredient();
                            dishing.Ingredient = i;
                            dishIngredients.Add(dishing);
                        }
                    }
                }
            }
            newDish.DishIngredients = dishIngredients;

            _context.Add(newDish);
            _context.SaveChanges();
            
            return _context.Dishes.LastOrDefault();
        }

        public Dish EditDish(AdminDishViewModel dish)
        {
            var dishToUpdate =  _context.Dishes.Include(c=>c.Category).SingleOrDefault(m => m.DishId == dish.DishId);
            var dishToUpdateDishIngredients = _context.DishIngredient.Where(x => x.DishId == dish.DishId).ToList();

            foreach (var item in dish.Ingredients)
            {
                if(item.IsChecked == true)
                {
                    //find ingrediens in DishIngredinens
                    var findIng = dishToUpdateDishIngredients.FirstOrDefault(x=>x.IngredientId == item.IngredientId);
                        if(findIng == null )
                    {
                        var ing = _context.DishIngredient.FirstOrDefault(x => x.Ingredient == item);
                        dishToUpdate.DishIngredients.Add(ing);
                       
                        
                    }
                    
                }
                if(item.IsChecked == false)
                {
                    var findIng = dishToUpdateDishIngredients.FirstOrDefault(x => x.IngredientId == item.IngredientId);
                    if (findIng != null)
                    {
                        var ing = _context.DishIngredient.FirstOrDefault(x => x.Ingredient == item);
                        dishToUpdate.DishIngredients.Remove(ing);
                        
                        
                    }

                }
            }

            dishToUpdate.Price = dish.Price;
            dishToUpdate.Name = dish.Name;
            dishToUpdate.ImagePath = dish.ImagePath;
            dishToUpdate.Category = dish.Category;

            _context.Update(dishToUpdate);
            _context.SaveChanges();
            return dishToUpdate;
        }
    }
}
