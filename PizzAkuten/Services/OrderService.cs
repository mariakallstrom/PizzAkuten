using System.Collections.Generic;
using System.Linq;
using PizzAkuten.Data;
using PizzAkuten.Models;
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
        public OrderDish SetOrderForCurrentSession(int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);
            // kolla om det finns en session, om det inte finns skapa en.

            if (_session.GetObjectFromJson<OrderDish>("Cart") == null)
            {
                var cart = new OrderDish();
                var item = new OrderItem();
                var itemList = new List<OrderItem>();
                item.Dish = dish;
                item.Quantity = 1;
                itemList.Add(item);
                cart.TotalPrice = item.Dish.Price;
                cart.OrderItems = itemList;

                _session.SetObjectAsJson("Cart", cart);
                return GetOrderForCurrentSession(_session);
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
                        cartFromSession.TotalPrice = tempDish.Dish.Price * tempDish.Quantity;
                    }

                    var index = cartFromSession.OrderItems.IndexOf(cartFromSession.OrderItems.First(x => x.Dish.DishId == dish.DishId));
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
                    item.Dish = dish;
                    item.Quantity = 1;
                    cartFromSession.OrderItems.Add(item);
                    cartFromSession.TotalPrice = cartFromSession.TotalPrice +=  item.Dish.Price;
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

            var orderItem = cartFromSession.OrderItems.SingleOrDefault(x => x.Dish.DishId == dishId);

            if(orderItem != null)
            {
                if(orderItem.Quantity > 1)
                {
                    cartFromSession.OrderItems.Where(x => x.Dish.DishId == dishId).First().Quantity -=1;
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

        public void MakeOrder()
        {
            var order = GetOrderForCurrentSession(_session);
        }

        //public void AddSpecialDishToCart(EditDishViewModel model)
        //{
        //    var specialDish = new Dish();
        //    var diList = new List<DishIngredient>();
        //    var xtraIngPrice = 0;

        //    foreach (var item in model.EditDish.DishIngredients)
        //    {
        //        var di = new DishIngredient();
        //        if (item.Ingredient.IsChecked)
        //        {
        //            di = item;
        //            di.Ingredient.Price = item.Ingredient.Price;
        //            di.Ingredient.Name = item.Ingredient.Name;
        //            diList.Add(di);
        //        }

        //    }

        //    foreach (var item in model.ExtraIngredients)
        //    {
        //        var di = new DishIngredient();
        //        if (item.IsChecked)
        //        {

        //            di.Ingredient = item.;
        //            di.Ingredient.Price = item.Ingredients.Price;
        //            xtraIngPrice = xtraIngPrice + item.Ingredients.Price;
        //            di.Ingredient.Name = item.Ingredients.Name;
        //            diList.Add(di);
        //        }

        //    }

            //var checkList = CheckForSpecialDishInSession();

            //if(checkList != null)
            //{
            //    specialDish.DishId = checkList.Last() + 1;
            //}
            //else
            //{
            //    specialDish.DishId = 90;
            //}

            specialDish.Price = model.EditDish.Price + xtraIngPrice;
            specialDish.DishIngredients = diList;
            specialDish.Name = model.EditDish.Name + " special";
            specialDish.SpecialDish = true;
            _context.Dishes.Add(specialDish);
            _context.SaveChanges();
            SetOrderForCurrentSession(specialDish.DishId);


        }

        //public IEnumerable<int> CheckForSpecialDishInSession()
        //{
        //    var session = GetOrderForCurrentSession(_session);
        //    if (session != null)
        //    {
             
        //        var checkForSpecialDish = session.OrderItems.Where(x => x.Dish.Name.Contains("special")).Select(p=>p.Dish.DishId);
        //        return checkForSpecialDish;
        //    }
        //    return null;
        //}
        }
    }
