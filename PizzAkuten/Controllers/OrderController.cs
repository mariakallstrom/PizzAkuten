﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Newtonsoft.Json;
using PizzAkuten.Models.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace PizzAkuten.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult AddToCart(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            // kolla om det finns en session, om det inte finns skapa en.
            if (GetSession("Cart") == null)
            {
                var cart = new List<CartItemModel>();
                var tempCart = CreateCart(cart, dish);

                SaveToSession(tempCart);
            }
            else
            {
                //kolla om maträtten finns i cart
                var cartFromSession = GetSession("Cart");
                var tempCart = JsonConvert.DeserializeObject<List<CartItemModel>>(cartFromSession);
                var dishExists = tempCart.Any(x => x.Dish.DishId == dish.DishId);

                if (dishExists)
                {
                    //om den finns addera antalet
                    var tempDish = tempCart.FirstOrDefault(x => x.Dish.DishId == dish.DishId);
                    if (tempDish != null)
                    {
                        tempDish.Quantity++;
                    }

                    var index = tempCart.IndexOf(tempCart.First(x => x.Dish.DishId == dish.DishId));
                    if (index != -1)
                    {
                        tempCart[index] = tempDish;
                        SaveToSession(tempCart);
                    }
                }
                else
                {
                    //om den inte finns lägg till den i cart                    
                    var tempSessionCart = CreateCart(tempCart, dish);
                    SaveToSession(tempSessionCart);
                }
            }

            return RedirectToAction("Index", "Menu");

        }

        private string GetSession(string sessionName)
        {
            return HttpContext.Session.GetString(sessionName);
        }

        private List<CartItemModel> CreateCart(List<CartItemModel> cart, Dish dish)
        {
            var cartItem = new CartItemModel
            {
                Quantity = 1,
                Dish = dish
            };

            cart.Add(cartItem);

            return cart;
        }

        private void SaveToSession(List<CartItemModel> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
        [AllowAnonymous]
        public IActionResult CheckOut()
        {
            var id = _userManager.GetUserId(User);
            var kund = _context.Users.SingleOrDefault(x => x.Id == id);

            if (GetSession("Cart") == null)
            {
                return RedirectToAction("Index", "Menu");
            }
            var temp = GetSession("Cart");
            List<CartItemModel> myShoppingCart = JsonConvert.DeserializeObject<List<CartItemModel>>(temp);

            foreach (var item in myShoppingCart)
            {
                if (item.Quantity > 1)
                {
                    item.Dish.Price = item.Dish.Price * item.Quantity;
                }
            }
            if (User.IsInRole("Premium") && kund.Points == 100)
            {
                var rabatt = myShoppingCart.Select(x => x.Dish).Min(x => x.Price);
                ViewBag.Discount = myShoppingCart.Sum(x => x.Dish.Price) - rabatt;
            }
            else
            {
                ViewBag.Discount = myShoppingCart.Sum(x => x.Dish.Price) * 0.8;
            }
            ViewBag.Sum = myShoppingCart.Sum(x => x.Dish.Price);


            return View(myShoppingCart);
        }

        [Authorize]
        public IActionResult Confirm()
        {
            return View("Checkout");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBuy()
        {
            var temp = GetSession("Cart");
            var myShoppingCart = JsonConvert.DeserializeObject<List<CartItemModel>>(temp);

            var id = _userManager.GetUserId(User);
            var customer = _context.Users.SingleOrDefault(x => x.Id == id);
            var b = new Order();


            if (User.IsInRole("Premium") && customer.Points == 100)
            {
                foreach (var item in myShoppingCart)
                {
                    if (item.Quantity > 1)
                    {
                        item.Dish.Price = item.Dish.Price * item.Quantity;
                    }
                }
                var rabatt = myShoppingCart.Select(x => x.Dish).Min(x => x.Price);
                var sum = myShoppingCart.Sum(x => x.Dish.Price);
                var total = sum - rabatt;
                b.TotalPrice = Convert.ToInt32(total);
                customer.Points = 0;
            }
            else if (User.IsInRole("Premium") && myShoppingCart.Sum(x => x.Quantity) >= 3)
            {
                foreach (var item in myShoppingCart)
                {
                    if (item.Quantity > 1)
                    {
                        item.Dish.Price = item.Dish.Price * item.Quantity;
                    }
                }
                var sum = myShoppingCart.Sum(x => x.Dish.Price) * 0.8;
                b.TotalPrice = Convert.ToInt32(sum);
                customer.Points = customer.Points + 10;
            }

            else
            {
                foreach (var item in myShoppingCart)
                {
                    if (item.Quantity > 1)
                    {
                        item.Dish.Price = item.Dish.Price * item.Quantity;
                    }
                }
                b.TotalPrice = myShoppingCart.Sum(x => x.Dish.Price);
            }

            foreach (var item in myShoppingCart)
            {
                var bm = new OrderDish();
                bm.DishId = item.Dish.DishId;
                bm.Quantity = item.Quantity;
                b.OrderDishes.Add(bm);
            }
            b.Delivered = false;
            b.OrderDate = DateTime.Now;
            b.ApplicationuserId = id;

            _context.Orders.Add(b);
            _context.SaveChanges();


            var model = _context.Orders.LastOrDefault();
            RemoveCart();
            return View(model);
        }

        public IActionResult RemoveCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveProduct(int dishId)
        {
            var temp = GetSession("Cart");
            var cart = JsonConvert.DeserializeObject<List<CartItemModel>>(temp);

            var selected = cart.FirstOrDefault(x => x.Dish.DishId.Equals(dishId));

            cart.Remove(selected);

            SaveToSession(cart);
            var value = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", value);

            var newtemp = GetSession("Cart");
            cart = JsonConvert.DeserializeObject<List<CartItemModel>>(newtemp);

            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Menu");
            }
            else
            {
                return RedirectToAction("CheckOut", cart);
            }
        }
    }
}
