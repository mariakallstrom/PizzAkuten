using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PizzAkuten.Controllers;
using PizzAkuten.Data;
using PizzAkuten.Models;
using PizzAkuten.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XUnitTestPizzAkuten.MockData;
using XUnitTestPizzAkuten.TestExtensions;

namespace XUnitTestPizzAkuten
{
    public class DishServiceTests
    {
        private readonly ApplicationDbContext _context;

        //public DishServiceTests()
        //{
        //    var db = new DbContextOptionsBuilder();
        //    db.UseInMemoryDatabase();
        //    _context = new MyContext(db.Options);
        //}
        //[Fact]
        //public void Get_A_Dish_Test()
        //{

        //    // Arrange
        //    var mockDbSet = new Mock<DbSet<Dish>>();
        //    var service = new DishService(_mockContext.Object, _mockHosting.Object);

        //    //Act
        //    var dishes = service.GetAllDishes().ToList();


        //    // Assert
        //    Assert.Equal(2, dishes.Count());
        //}
    }
}
