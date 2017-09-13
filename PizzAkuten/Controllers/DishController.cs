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
using Microsoft.Extensions.Logging;

namespace PizzAkuten.Controllers
{
    [Authorize(Roles ="admin")]
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DishService _service;
        private readonly ILogger _logger;


        public DishController(IHostingEnvironment environment, DishService service, ApplicationDbContext context, ILogger<DishController> logger)
        {
            _hostingEnvironment = environment;
            _service = service;
            _context = context;
            _logger = logger;
        }

   

        // GET: Dish
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.Include(c => c.Category).Include(d => d.DishIngredients).ToListAsync();
            if (dishes == null)
            {
                _logger.LogWarning("No dishes Found");
            }
            return View(dishes);
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
                _logger.LogWarning("No dish was found with id =" + id);
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dish/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dish/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            if (form == null)
            {
                return NotFound();
            }

            _service.SaveDishToDatabase(form);
            return RedirectToAction(nameof(Index));

        }

        // GET: Dish/Edit/5
        public async Task<IActionResult> Edit(int? DishId)
        {
            if (DishId == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(c=>c.Category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
              .SingleOrDefaultAsync(m => m.DishId == DishId);
 
            foreach (var item in dish.DishIngredients)
            {
                item.Ingredient.IsChecked = true;
            }
           
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IFormCollection form)
        {
            _service.EditDish(form);
            return RedirectToAction(nameof(Index));
        }

        // GET: Dish/Delete/5
        public async Task<IActionResult> Delete(int? DishId)
        {
            if (DishId == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == DishId);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int DishId)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == DishId);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int DishId)
        {
            return _context.Dishes.Any(e => e.DishId == DishId);
        }

   
    }
}
