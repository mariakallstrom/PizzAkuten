﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PizzAkuten.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;

namespace PizzAkuten.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OrderService _service;
      

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, OrderService service, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
            _signInManager = signInManager;
        
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

            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).Include(dei=>dei.DishExtraIngredients).ThenInclude(di=>di.ExtraIngredient)
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
            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == dishId);

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

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {
            if (form.Keys.Count != 0)
            {
                _service.AddSpecialDishToCart(form);
                return RedirectToAction("Index", "Home");
            };
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewOrder(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        public async Task<IActionResult> ConfirmOrder(IFormCollection form)
        {
            if (form.Keys.Count != 0)
            {
                var model = new NonAccountUser();
                model.FirstName = form["FirstName"];
                model.LastName = form["LastName"];
                model.Street = form["Street"];
                model.ZipCode = Convert.ToInt32(form["ZipCode"]);
                model.Phone = form["Phone"];
                model.Email = form["Email"];
                model.City = form["City"];
                _context.NonAccountUsers.Add(model);
                _context.SaveChanges();
            };

            var order = _service.GetOrder();
            if(_signInManager.IsSignedIn(User))
            {
                var currentUser = User;
                order.ApplicationUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var newOrder = _service.SaveOrderToDataBase(order);
              

                return RedirectToAction("Create", "Payment", new {id = newOrder.OrderId});
            }
            else if(!_signInManager.IsSignedIn(User))
            {
                order.NonAccountUser = _context.NonAccountUsers.Last();
                var savedOrder = _service.SaveOrderToDataBase(order);
                return RedirectToAction("Create", "Payment", new { id = savedOrder.OrderId });
            }

            return RedirectToAction("Create", "Payment");


        }
 

    }
}
