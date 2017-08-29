using System.Collections.Generic;
using System.Linq;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PizzAkuten.Extensions;
using System;
using System.Security.Claims;

namespace PizzAkuten.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public OrderService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public OrderDish SetOrderForCurrentSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
     
            // kolla om det finns en session, om det inte finns skapa en.

            if (_session.GetObjectFromJson<OrderDish>("Cart") == null)
            {
                var cart = new OrderDish();
                var item = new OrderItem();
                var itemList = new List<OrderItem>();
                item.OrderDish = dish;
                item.Quantity = 1;
                itemList.Add(item);
                cart.TotalPrice = item.OrderDish.Price;
                cart.OrderItems = itemList;

                _session.SetObjectAsJson("Cart", cart);
               return  GetOrderForCurrentSession(_session);
               
            }
            else
            {
                //kolla om maträtten finns i cart
                var cartFromSession = _session.GetObjectFromJson<OrderDish>("Cart");
                var dishExists = cartFromSession.OrderItems.Any(x => x.OrderDish.DishId == dish.DishId);

                if (dishExists)
                {
                    //om den finns addera antalet
                    var tempDish = cartFromSession.OrderItems.FirstOrDefault(x => x.OrderDish.DishId == dish.DishId);
                    if (tempDish != null)
                    {
                        tempDish.Quantity++;
                        cartFromSession.TotalPrice = cartFromSession.TotalPrice + tempDish.OrderDish.Price;
                    }

                    var index = cartFromSession.OrderItems.IndexOf(cartFromSession.OrderItems.First(x => x.OrderDish.DishId == dish.DishId));
                    if (index != -1)
                    {
                        cartFromSession.OrderItems[index] = tempDish;
                       
                        _session.SetObjectAsJson("Cart", cartFromSession);
                        return GetOrderForCurrentSession(_session);
                    }
                }
                else
                {
                    //om den inte finns lägg till den i cart 
                    var cart = new OrderDish();
                    var item = new OrderItem();
                    item.OrderDish = dish;
                    item.Quantity = 1;
                    cartFromSession.OrderItems.Add(item);
                    cartFromSession.TotalPrice = cartFromSession.TotalPrice + item.OrderDish.Price;
                    _session.SetObjectAsJson("Cart", cartFromSession);
                    return GetOrderForCurrentSession(_session);
                }
            }

            return GetOrderForCurrentSession(_session);
        }

        public OrderDish GetOrderForCurrentSession(ISession session)
        {
            var orderDish = _session.GetObjectFromJson<OrderDish>("Cart");
            if(orderDish != null)
            {
                return orderDish;
            }
            return null;
          
        }

        public OrderDish RemoveItemFromSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            var cartFromSession = _session.GetObjectFromJson<OrderDish>("Cart");

            var orderItem = cartFromSession.OrderItems.SingleOrDefault(x => x.OrderDish.DishId == dishId);

            if(orderItem != null)
            {
                if(orderItem.Quantity > 1)
                {
                    cartFromSession.OrderItems.Where(x => x.OrderDish.DishId == dishId).First().Quantity -=1;
                    cartFromSession.TotalPrice = cartFromSession.TotalPrice -= dish.Price;
                    _session.SetObjectAsJson("Cart", cartFromSession);
                    return GetOrderForCurrentSession(_session);
                }
                cartFromSession.OrderItems.Remove(orderItem);
                cartFromSession.TotalPrice = cartFromSession.TotalPrice -= dish.Price;
                _session.SetObjectAsJson("Cart", cartFromSession);
                return GetOrderForCurrentSession(_session);

            }
            return GetOrderForCurrentSession(_session);
        }

        public Order GetOrder()
        {
            var orderList = GetOrderForCurrentSession(_session);
            var order = new Order();
            var orderDish = new OrderDish();
            var orderItemlist = new List<OrderItem>();

            foreach (var item in orderList.OrderItems)
            {
                var orderItem = new OrderItem();
                var dish = _context.Dishes.FirstOrDefault(x => x.DishId == item.OrderDish.DishId);
                orderItem.OrderDish = dish;
                orderItem.Quantity = item.Quantity;
                
                orderItemlist.Add(orderItem);
                


            }
          
            orderDish.OrderItems = orderItemlist;
            order.OrderDish = orderDish;

            order.OrderDate = DateTime.Today;
            order.TotalPrice = orderList.TotalPrice;
            return order;
        }

        public Order SaveOrderToDataBase(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
            var newOrder = _context.Orders.Last();

            return newOrder;
        }

        public void DeleteSpecialDishes(Order order)
        {
            foreach (var item in order.OrderDish.OrderItems)
            {
                if(item.OrderDish.Name.Contains("special"))
                {
                    var dishes = _context.Dishes.Where(x => x.DishId == item.OrderDish.DishId);
                    foreach (var dish in dishes)
                    {
                        _context.Dishes.Remove(dish);
                        _context.SaveChangesAsync();
                    }
                }
            }
        }
        public void AddSpecialDishToCart(EditDishViewModel model)
        {
            //Skapa en ny Special Dish
            var specialDish = new Dish();
            var diList = new List<DishIngredient>();
            var xDiList = new List<DishExtraIngredient>();
            var xtraIngPrice = 0;

            //Kolla vilka vanliga ingredienser som är bockade 
            foreach (var item in model.EditDish.DishIngredients.ToList())
            {
                if (item.Ingredient.IsChecked)
                {
                    diList.Add(item);
                }
            }
            //Kolla vilka extra ingredienser som är bockade
            foreach (var item in model.ExtraIngredients.ToList())
            {
                var di = new DishExtraIngredient();
                if (item.IsChecked)
                {
                    di.ExtraIngredient = item;
                    xtraIngPrice = xtraIngPrice + item.Price;

                    xDiList.Add(di);
                }
            }

            specialDish.Price = model.EditDish.Price + xtraIngPrice;
            specialDish.DishExtraIngredients = xDiList;
            specialDish.DishIngredients = diList;
            specialDish.Name = model.EditDish.Name + " special";
            specialDish.SpecialDish = true;

            _context.Add(specialDish);
            _context.SaveChangesAsync();

            SetOrderForCurrentSession(specialDish.DishId);
        }
    }
}
