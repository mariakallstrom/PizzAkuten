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


    }
}
