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
        private readonly ISession _session;

        public OrderService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }
        public Cart SetOrderForCurrentSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
     
            // kolla om det finns en session, om det inte finns skapa en.
            if (_session.GetObjectFromJson<Cart>("Cart") == null)
            {
                return CreateSession(dish);
            }
            else
            {
                //kolla om maträtten finns i cart
                var cartFromSession = _session.GetObjectFromJson<Cart>("Cart");
                var dishExists = cartFromSession.CartItems.Any(x => x.Dish.DishId == dish.DishId);
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
        public Cart CreateSession(Dish dish)
        {
            var cart = new Cart();
            var item = new CartItem();
            var itemList = new List<CartItem>();
            item.Dish = dish;
            item.Quantity = 1;
            itemList.Add(item);
            cart.TotalPrice = item.Dish.Price;
            cart.CartItems = itemList;

            _session.SetObjectAsJson("Cart", cart);
            return GetCurrentOrder();
        }
        public Cart AddDishToCart(Cart cartFromSession, Dish dish)
        {
            var cart = new Cart();
            var item = new CartItem();
            item.Dish = dish;
            item.Quantity = 1;
            cartFromSession.CartItems.Add(item);
            cartFromSession.TotalPrice = cartFromSession.TotalPrice + item.Dish.Price;
            _session.SetObjectAsJson("Cart", cartFromSession);
            return GetCurrentOrder();
        }
        public Cart AddQuantityToDish(Cart cartFromSession, Dish dish)
        {
            var tempDish = cartFromSession.CartItems.FirstOrDefault(x => x.Dish.DishId == dish.DishId);
            if (tempDish != null)
            {
                tempDish.Quantity++;
                cartFromSession.TotalPrice = cartFromSession.TotalPrice + tempDish.Dish.Price;
            }

            var index = cartFromSession.CartItems.IndexOf(cartFromSession.CartItems.First(x => x.Dish.DishId == dish.DishId));
            if (index != -1)
            {
                cartFromSession.CartItems[index] = tempDish;

                _session.SetObjectAsJson("Cart", cartFromSession);
                return GetCurrentOrder();
            }

            return null;
        }
        public Cart GetCurrentOrder()
        {
            var Cart = _session.GetObjectFromJson<Cart>("Cart");
            if(Cart != null)
            {
                return Cart;
            }
            return null;
          
        }
        public Cart RemoveItemFromSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            var cartFromSession = _session.GetObjectFromJson<Cart>("Cart");
            var orderItem = cartFromSession.CartItems.SingleOrDefault(x => x.Dish.DishId == dishId);

            if(orderItem != null)
            {
                if(orderItem.Quantity > 1)
                {
                    cartFromSession.CartItems.Where(x => x.Dish.DishId == dishId).First().Quantity -=1;
                    cartFromSession.TotalPrice = cartFromSession.TotalPrice -= dish.Price;
                    _session.SetObjectAsJson("Cart", cartFromSession);
                    return GetCurrentOrder();
                }
                cartFromSession.CartItems.Remove(orderItem);
                cartFromSession.TotalPrice = cartFromSession.TotalPrice -= dish.Price;
                _session.SetObjectAsJson("Cart", cartFromSession);
                return GetCurrentOrder();
            }
            return GetCurrentOrder();
        }
        public Order GetOrder()
        {
            var orderList = GetCurrentOrder();
            if (orderList == null)
            {
                return null;
            }
            var order = new Order();
            var Cart = new Cart();
            var orderItemlist = new List<CartItem>();

            foreach (var item in orderList.CartItems)
            {
                var orderItem = new CartItem();
                var dish = _context.Dishes.FirstOrDefault(x => x.DishId == item.Dish.DishId);
                orderItem.Dish = dish;
                orderItem.Quantity = item.Quantity;
                orderItemlist.Add(orderItem);
            }
          
            Cart.CartItems = orderItemlist;
            order.Cart = Cart;
            order.OrderDate = DateTime.Today;
            order.TotalPrice = orderList.TotalPrice;
            return order;
        }
        public Order SaveOrderToDataBase(Order order)
        {
            var newOrder = new Order {
                Cart = order.Cart,
                OrderDate = DateTime.Now,
                TotalPrice = order.TotalPrice,
            };
            if(order.ApplicationUser != null)
            {
                newOrder.ApplicationUser = order.ApplicationUser;
            }
            else { newOrder.NonAccountUser = order.NonAccountUser; }

            _context.Add(order);
            _context.SaveChanges();
            var thisOrder = _context.Orders.Last();

            return thisOrder;
        }
        public void DeleteSpecialDishes(Order order)
        {
            foreach (var item in order.Cart.CartItems)
            {
                if(item.Dish.Name.Contains("special"))
                {
                    var dishes = _context.Dishes.Where(x => x.DishId == item.Dish.DishId);
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
