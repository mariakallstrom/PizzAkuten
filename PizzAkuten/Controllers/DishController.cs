﻿using System;
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
    

        public DishController(IHostingEnvironment environment, DishService service)
        {
            _hostingEnvironment = environment;
            _service = service;
        }

        public DishController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("DishId,Name,Price, CategoryId, ImageFile, Ingredients")] AdminDishViewModel dish, IFormFile file)
        {
            if(file != null)
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

                dish.ImagePath = "/images/" + file.FileName;
            }
         
            if (ModelState.IsValid)
            {
                _service.SaveDishToDatabase(dish);
                return RedirectToAction(nameof(Index));
            }
            return View();
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
            var editDish = new AdminDishViewModel();
            editDish.DishId = dish.DishId;
            editDish.Name = dish.Name;
            editDish.Price = dish.Price;
            editDish.Category = dish.Category;
            editDish.DishIngredients = dish.DishIngredients;
            editDish.ImagePath = dish.ImagePath;

            var ingredients = _context.Ingredients.ToList();
            editDish.Ingredients = ingredients;

            foreach (var item in dish.DishIngredients)
            {
                item.Ingredient.IsChecked = true;
            }

            var categories = _context.Categories.ToList();
            editDish.SelectCategoryList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                var category = new SelectListItem { Text = item.Name, Value = item.CategoryId.ToString() };
                editDish.SelectCategoryList.Add(category);
               
            }
           
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
        public async Task<IActionResult> Edit([Bind("DishId,Name,Price, CategoryId, ImageFile, Ingredients")] AdminDishViewModel dish, IFormFile file)
        {
            if (dish == null)
            {
                return NotFound();
            }
            if (file != null)
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

                dish.ImagePath = "/images/" + file.FileName;
            }
            _service.EditDish(dish);

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
