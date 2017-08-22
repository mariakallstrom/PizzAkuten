using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PizzAkuten.Controllers;
using PizzAkuten.Data;
using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XUnitTestPizzAkuten.TestExtensions;

namespace XUnitTestPizzAkuten
{
    public class DishControllerTests
    {
        [Fact]
        public async void Test1Async()
        {
            //Arrange
            var dishes = GetFakeDishes().ToAsyncDbSetMock();

            var mockDbContext = new Mock<ApplicationDbContext>();
            var mockDbSet = new Mock<DbSet<Dish>>();
            mockDbContext.Setup(repo => repo.Dishes).Returns(dishes.Object);
            var controller = new DishController(mockDbContext.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Dish>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private List<Dish> GetFakeDishes()
        {
            return new List<Dish>()
            {
                new Dish()
                {
                    DishId = 1,
                    Name = "Hawaii"
                },
        new Dish()
        {
            DishId = 2,
            Name = "Vezuvio"
        }
            };
        }
    }
}
