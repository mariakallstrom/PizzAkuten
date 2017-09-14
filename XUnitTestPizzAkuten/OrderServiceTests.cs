using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PizzAkuten.Data;
using PizzAkuten.Services;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzAkuten.Models;
using Xunit;
using XUnitTestPizzAkuten.FakeData;
using XUnitTestPizzAkuten.Fakes;

namespace XUnitTestPizzAkuten
{
    public class OrderServiceTests : BaseClassFakeData
    {

        public OrderServiceTests()
        {
            base.FakeDataInitializer();
        }

        [Fact]
        public void Add_Dish_To_Cart()
        {
            //arrange
            
            var dService = _serviceProvider.GetService<DishService>();
            var service = _serviceProvider.GetService<OrderService>();

            //act
            var dish = dService.GetAllDishes().FirstOrDefault(x => x.DishId == 1);
            var cart = new Cart();
            cart.CartItems = new List<CartItem>{new CartItem{Dish = dish, Quantity = 1}};
            var result = service.AddDishToCart(cart, dish);
            //assert
            Assert.Equal(cart, result);
        }
        [Fact]
        public void Add_Dish_To_Cart_Quantity_2()
        {

            //arrange
            var service = _serviceProvider.GetService<OrderService>();
            var dService = _serviceProvider.GetService<DishService>();

            //act
            var dish = dService.GetAllDishes().FirstOrDefault(x => x.DishId == 1);
            var Q2 = 2;

            var newCart = new Cart();
            newCart.CartItems = new List<CartItem> { new CartItem { Dish = dish, Quantity = 1 } };
            var cart2 = service.AddQuantityToDish(newCart, dish);
            //assert
            var Q1 = cart2.CartItems.Select(x => x.Quantity).Single();
            Assert.Equal(Q1, Q2);
        }
    }
}
