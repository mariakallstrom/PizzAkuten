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
        public void AddToCart(int dishId)
        {
            if(dishId == 0)
            {
                RedirectToAction("Index", "Home");
            }
            _service.SetOrderForCurrentSession(dishId);
            //var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            //// kolla om det finns en session, om det inte finns skapa en.
            //if (GetSession("Cart") == null)
            //{
            //    var cart = new DishCartViewModel();
            //    var tempCart = CreateCart(cart, dish);

            //    SaveToSession(tempCart);
            //    return PartialView("_CartPartial", tempCart);
            //}
            //else
            //{
            //    //kolla om maträtten finns i cart
            //    var cartFromSession = GetSession("Cart");
            //    var tempCart = JsonConvert.DeserializeObject<DishCartViewModel>(cartFromSession);
            //    var dishExists = tempCart.CartItemModel.Any(x => x.Dish.DishId == dish.DishId);

            //    if (dishExists)
            //    {
            //        //om den finns addera antalet
            //        var tempDish = tempCart.CartItemModel.FirstOrDefault(x => x.Dish.DishId == dish.DishId);
            //        if (tempDish != null)
            //        {
            //            tempDish.Quantity++;
            //        }

            //        var index = tempCart.CartItemModel.IndexOf(tempCart.CartItemModel.First(x => x.Dish.DishId == dish.DishId));
            //        if (index != -1)
            //        {
            //            tempCart.CartItemModel[index] = tempDish;
            //            SaveToSession(tempCart);
            //        }
            //        return PartialView("_CartPartial", tempCart);
            //    }
            //    else
            //    {
            //        //om den inte finns lägg till den i cart                    
            //        var tempSessionCart = CreateCart(tempCart, dish);
            //        SaveToSession(tempSessionCart);
            //        return PartialView("_CartPartial", tempCart);
            //    }
            //}

        }

        //private string GetSession(string sessionName)
        //{
        //    return HttpContext.Session.GetString(sessionName);
        //}

        //private DishCartViewModel CreateCart(DishCartViewModel cart, Dish dish)
        //{
        //    var cartItem = new CartItemModel
        //    {
        //        Quantity = 1,
        //        Dish = dish
        //    };

        //    cart.CartItemModel.Add(cartItem);

        //    return cart;
        //}

        //private void SaveToSession(DishCartViewModel cart)
        //{
        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        //}
        //[AllowAnonymous]
        //public IActionResult CheckOut()
        //{
        //    var id = _userManager.GetUserId(User);
        //    var kund = _context.Users.SingleOrDefault(x => x.Id == id);

        //    if (GetSession("Cart") == null)
        //    {
        //        return RedirectToAction("Index", "Menu");
        //    }
        //    var temp = GetSession("Cart");
        //    DishCartViewModel myShoppingCart = JsonConvert.DeserializeObject<DishCartViewModel>(temp);

        //    foreach (var item in myShoppingCart.CartItemModel)
        //    {
        //        if (item.Quantity > 1)
        //        {
        //            item.Dish.Price = item.Dish.Price * item.Quantity;
        //        }
        //    }
        //    if (User.IsInRole("Premium") && kund.Points == 100)
        //    {
        //        var rabatt = myShoppingCart.CartItemModel.Select(x => x.Dish).Min(x => x.Price);
        //        ViewBag.Discount = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price) - rabatt;
        //    }
        //    else
        //    {
        //        ViewBag.Discount = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price) * 0.8;
        //    }
        //    ViewBag.Sum = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price);


        //    return View(myShoppingCart);
        //}

        //[Authorize]
        //public IActionResult Confirm()
        //{
        //    return View("Checkout");
        //}
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ConfirmBuy()
        //{
        //    var temp = GetSession("Cart");
        //    var myShoppingCart = JsonConvert.DeserializeObject<DishCartViewModel>(temp);

        //    var id = _userManager.GetUserId(User);
        //    var customer = _context.Users.SingleOrDefault(x => x.Id == id);
        //    var b = new Order();


        //    if (User.IsInRole("Premium") && customer.Points == 100)
        //    {
        //        foreach (var item in myShoppingCart.CartItemModel)
        //        {
        //            if (item.Quantity > 1)
        //            {
        //                item.Dish.Price = item.Dish.Price * item.Quantity;
        //            }
        //        }
        //        var rabatt = myShoppingCart.CartItemModel.Select(x => x.Dish).Min(x => x.Price);
        //        var sum = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price);
        //        var total = sum - rabatt;
        //        b.TotalPrice = Convert.ToInt32(total);
        //        customer.Points = 0;
        //    }
        //    else if (User.IsInRole("Premium") && myShoppingCart.CartItemModel.Sum(x => x.Quantity) >= 3)
        //    {
        //        foreach (var item in myShoppingCart.CartItemModel)
        //        {
        //            if (item.Quantity > 1)
        //            {
        //                item.Dish.Price = item.Dish.Price * item.Quantity;
        //            }
        //        }
        //        var sum = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price) * 0.8;
        //        b.TotalPrice = Convert.ToInt32(sum);
        //        customer.Points = customer.Points + 10;
        //    }

        //    else
        //    {
        //        foreach (var item in myShoppingCart.CartItemModel)
        //        {
        //            if (item.Quantity > 1)
        //            {
        //                item.Dish.Price = item.Dish.Price * item.Quantity;
        //            }
        //        }
        //        b.TotalPrice = myShoppingCart.CartItemModel.Sum(x => x.Dish.Price);
        //    }

        //    foreach (var item in myShoppingCart.CartItemModel)
        //    {
        //        var bm = new OrderDish();
        //        bm.DishId = item.Dish.DishId;
        //        bm.Quantity = item.Quantity;
        //        b.OrderDishes.Add(bm);
        //    }
        //    b.Delivered = false;
        //    b.OrderDate = DateTime.Now;
        //    b.ApplicationuserId = id;

        //    _context.Orders.Add(b);
        //    _context.SaveChanges();


        //    var model = _context.Orders.LastOrDefault();
        //    RemoveCart();
        //    return View(model);
        //}

        //public IActionResult RemoveCart()
        //{
        //    HttpContext.Session.Remove("Cart");
        //    return RedirectToAction("Index", "Home");
        //}

        //public IActionResult RemoveProduct(int dishId)
        //{
        //    var temp = GetSession("Cart");
        //    var cart = JsonConvert.DeserializeObject<DishCartViewModel>(temp);

        //    var selected = cart.CartItemModel.FirstOrDefault(x => x.Dish.DishId.Equals(dishId));

        //    cart.CartItemModel.Remove(selected);

        //    SaveToSession(cart);
        //    var value = JsonConvert.SerializeObject(cart);
        //    HttpContext.Session.SetString("Cart", value);

        //    var newtemp = GetSession("Cart");
        //    cart = JsonConvert.DeserializeObject<DishCartViewModel>(newtemp);

        //    if (cart.CartItemModel.Count == 0)
        //    {
        //        return RedirectToAction("Index", "Menu");
        //    }
        //    else
        //    {
        //        return RedirectToAction("CheckOut", cart);
        //    }
        //}
    }
}
