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
                return CreateSession(dish);
            }
            else
            {
                //kolla om maträtten finns i cart
                var cartFromSession = _session.GetObjectFromJson<OrderDish>("Cart");
                var dishExists = cartFromSession.OrderItems.Any(x => x.OrderDish.DishId == dish.DishId);
                if (dishExists)
                {
                    //om den finns addera antalet
                    return AddQuantityToDish(cartFromSession, dish);
                }
                else
                {
                    //om den inte finns lägg till den i cart
                    return AddDishToCart(cartFromSession, dish);
                }
            }
        }
        public OrderDish CreateSession(Dish dish)
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
            return GetOrderForCurrentSession(_session);
        }
        public OrderDish AddDishToCart(OrderDish cartFromSession, Dish dish)
        {
            var cart = new OrderDish();
            var item = new OrderItem();
            item.OrderDish = dish;
            item.Quantity = 1;
            cartFromSession.OrderItems.Add(item);
            cartFromSession.TotalPrice = cartFromSession.TotalPrice + item.OrderDish.Price;
            _session.SetObjectAsJson("Cart", cartFromSession);
            return GetOrderForCurrentSession(_session);
        }
        public OrderDish AddQuantityToDish(OrderDish cartFromSession, Dish dish)
        {
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

            return null;
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
        public void AddSpecialDishToCart(IFormCollection form)
        {
            var dish = _context.Dishes.Find(Convert.ToInt32(form["DishId"]));
            //Skapa en ny Special Dish
            var specialDish = new Dish();
            var xtraIngPrice = 0;
            var diList = CheckOrdinaryIngredientsForSpecialDish(form, new List<DishIngredient>());
            var xDiList = CheckExtraIngredientsForSpecialDish(form, new List<DishExtraIngredient>());

            if(xDiList != null)
            {
                foreach (var item in xDiList)
                {
                    var price = _context.ExtraIngredients.Find(item.ExtraIngredientId).Price;
                    xtraIngPrice = xtraIngPrice + price;
                }
                specialDish.DishExtraIngredients = xDiList;
            }
            if(diList != null)
            {
                specialDish.DishIngredients = diList;
            }
            specialDish.Price = dish.Price + xtraIngPrice;
            specialDish.Name = dish.Name + " special";
            specialDish.SpecialDish = true;

            _context.Add(specialDish);
            _context.SaveChanges();

            SetOrderForCurrentSession(specialDish.DishId);
        }
        public List<DishIngredient> CheckOrdinaryIngredientsForSpecialDish(IFormCollection form, List<DishIngredient> diList)
        {
            //Kolla vilka vanliga ingredienser som är bockade 
            var ordinaryKeys = form.Keys.FirstOrDefault(k => k.Contains("OrdinaryChecked-"));
            if(ordinaryKeys!= null)
            {
                var getId = ordinaryKeys.IndexOf("-");
                var checkedIngredients = form.Keys.Where(k => k.Contains("OrdinaryChecked-"));

                //Lägg till vanliga ingredienserna
                foreach (var ingredient in checkedIngredients)
                {
                    var id = int.Parse(ingredient.Substring(getId + 1));
                    var ing = new DishIngredient { IngredientId = id };
                    diList.Add(ing);
                    _context.DishIngredients.Add(ing);
                }

                return diList;
            }
            return null;
        }
        public List<DishExtraIngredient> CheckExtraIngredientsForSpecialDish(IFormCollection form, List<DishExtraIngredient> xDiList)
        {
            //Kolla vilka extra ingredienser som är bockade 

            var extraKeys = form.Keys.FirstOrDefault(k => k.Contains("ExtraChecked-"));
            if(extraKeys != null)
            {
                var getId = extraKeys.IndexOf("-");
                var checkedExtraIngredients = form.Keys.Where(k => k.Contains("ExtraChecked-"));

                //Lägg till extra ingredienserna
                foreach (var ingredient in checkedExtraIngredients)
                {
                    var id = int.Parse(ingredient.Substring(getId + 1));
                    var extraIng = new DishExtraIngredient { ExtraIngredientId = id };
                    xDiList.Add(extraIng);
                    _context.DishExtraIngredients.Add(extraIng);
                }
                return xDiList;
            }
            return null;
     
        }
        public void DeleteSession()
        {
            _session.Remove("Cart");
        }
    }
}
