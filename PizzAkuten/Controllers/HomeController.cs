﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzAkuten.Models;
using PizzAkuten.Data;
using Microsoft.EntityFrameworkCore;

namespace PizzAkuten.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public  HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.Include("Category").Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .ToListAsync();

            foreach (var item in dishes)
            {
                if(item.SpecialDish)
                {
                    dishes.Remove(item);
                }
            }

            if (dishes == null)
            {
                return NotFound();
            }
           
            return View(dishes);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
