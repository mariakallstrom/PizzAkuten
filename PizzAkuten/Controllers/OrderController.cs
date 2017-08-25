using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PizzAkuten.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PizzAkuten.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OrderService _service;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, OrderService service)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
        }
        [AllowAnonymous]
        public IActionResult AddToCart(int dishId)
        {
            if(dishId == 0)
            {
                RedirectToAction("Index", "Home");
            }
            _service.SetOrderForCurrentSession(dishId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromCart(int dishId)
        {
            if (dishId == 0)
            {
                RedirectToAction("Index", "Home");
            }
            _service.RemoveItemFromSession(dishId);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int dishId)
        {
            if (dishId == 0)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == dishId);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        public async Task<IActionResult> Edit(int dishId)
        {
            if (dishId == 0)
            {
                return NotFound();
            }
            var ingredients = _context.Ingredients.ToList();
            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == dishId);

            var model = new EditDishViewModel();
       
            var ingList = new List<ExtraIngredient>();

            foreach (var item in ingredients)
            {
                var ing = new ExtraIngredient();
                item.IsChecked = true;
                ing.Ingredients = item;
                ingList.Add(ing);
            }
            model.EditDish = dish;
            model.ExtraIngredients = ingList;

            if (dish == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditDishViewModel model)
        {
            if(model != null)
            {
                _service.AddSpecialDishToCart(model);

            };

            return null;
        }
    }
}
