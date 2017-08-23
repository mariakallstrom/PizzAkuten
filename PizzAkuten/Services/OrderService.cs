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
using PizzAkuten.Extensions;

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
        public void SetOrderForCurrentSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            // kolla om det finns en session, om det inte finns skapa en.

            if (_session.GetObjectFromJson<OrderDish>("Cart") == null)
            {
                var cart = new OrderDish();
                var item = new OrderItem();
                var itemList = new List<OrderItem>();
                item.Dish = dish;
                itemList.Add(item);
                cart.OrderItems = itemList;

                _session.SetObjectAsJson("Cart", cart);
            }
            else
            {
                //kolla om maträtten finns i cart
                var cartFromSession = _session.GetObjectFromJson<OrderDish>("Cart");
                var dishExists = cartFromSession.OrderItems.Any(x => x.Dish.DishId == dish.DishId);

                if (dishExists)
                {
                    //om den finns addera antalet
                    var tempDish = cartFromSession.OrderItems.FirstOrDefault(x => x.Dish.DishId == dish.DishId);
                    if (tempDish != null)
                    {
                        tempDish.Quantity++;
                    }

                    var index = cartFromSession.OrderItems.IndexOf(cartFromSession.OrderItems.First(x => x.Dish.DishId == dish.DishId));
                    if (index != -1)
                    {
                        cartFromSession.OrderItems[index] = tempDish;
                        cartFromSession.OrderItems.Add(tempDish);
                        _session.SetObjectAsJson("Cart", cartFromSession);
                    }
                }
                else
                {
                    //om den inte finns lägg till den i cart 
                    var cart = new OrderDish();
                    var item = new OrderItem();
                    item.Dish = dish;
                    cartFromSession.OrderItems.Add(item);
                    _session.SetObjectAsJson("Cart", cartFromSession);
                }
            }
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


        }
    }
