﻿using System;
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
        public void Create_New_Session_And_Add_Dish_To_Cart()
        {

            //arrange
            var service = _serviceProvider.GetService<OrderService>();
            var dService = _serviceProvider.GetService<DishService>();

            //act
            var dish = dService.GetAllDishes().FirstOrDefault(x => x.DishId == 1);
            var result =  service.SetOrderForCurrentSession(1);
            var cart = new Cart();
            cart.CartItems = new List<CartItem>{new CartItem{Dish = dish, Quantity = 1}};

            //assert
            Assert.Equal(cart, result);
        }

        public void Update_Session_And_Add_Dish_To_Cart_Quantity_2()
        {

            //arrange
            var service = _serviceProvider.GetService<OrderService>();
            var dService = _serviceProvider.GetService<DishService>();

            //act
            var dish = dService.GetAllDishes().FirstOrDefault(x => x.DishId == 1);
            service.SetOrderForCurrentSession(1);
            var result = service.SetOrderForCurrentSession(1);
            var cart = new Cart();
            cart.CartItems = new List<CartItem> { new CartItem { Dish = dish, Quantity = 2 } };

            //assert
            Assert.Equal(cart, result);
        }


    }
}
