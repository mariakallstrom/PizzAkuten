using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PizzAkuten.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using PizzAkuten.Services;
using System.Collections.Generic;

namespace PizzAkuten.Controllers
{
    [Authorize(Roles ="admin")]
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DishService _service;

        public DishController(ApplicationDbContext context, IHostingEnvironment environment, DishService service)
        {
            _context = context;
            _hostingEnvironment = environment;
            _service = service;
        }

        // GET: Dish
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.Dishes.Include(c=>c.Category).Include(d=>d.DishIngredients).ToListAsync());
        }

        // GET: Dish/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dish/Create
        public IActionResult Create()
        {
            var model = new AdminDishViewModel();
            var categories = _context.Categories.ToList();
            model.SelectCategoryList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                var category = new SelectListItem { Text = item.Name, Value = item.CategoryId.ToString() };
                model.SelectCategoryList.Add(category);
            }
            model.Ingredients = _context.Ingredients.ToList();

            return View(model);
        }

        // POST: Dish/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Price, CategoryId, Ingredients, ImageFile, Ingredients")] AdminDishViewModel dish, IFormFile file)
        {
            var upload = Path.Combine(_hostingEnvironment.WebRootPath, "images");

            if (!_service.CheckIfImageExistsInImageFolder(file))
            {
                if (file.Length > 0)
                {
                    var fileStream = new FileStream(Path.Combine(upload, file.FileName), FileMode.Create);
                    await file.CopyToAsync(fileStream);
                }
            }
               
            dish.ImagePath = "images/" + file.FileName;
            if (ModelState.IsValid)
            {
                _service.SaveDishToDatabase(dish);
                return RedirectToAction(nameof(Index));
              
           
            }
            return View();
        }

        // GET: Dish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            var editDish = new AdminDishViewModel
            {
                Name = dish.Name,
                Price = dish.Price,
                ImagePath = dish.ImagePath,
                Category = dish.Category,
                DishIngredients = dish.DishIngredients,
                DishId = dish.DishId,
                CategoryId = dish.CategoryId

            };
            if (dish == null)
            {
                return NotFound();
            }
            return View(editDish);
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,Price")] Dish dish)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }

   
    }
}
