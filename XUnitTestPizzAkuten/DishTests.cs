using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PizzAkuten.Data;
using PizzAkuten.Models;
using PizzAkuten.Services;
using System.Linq;
using PizzAkuten.Controllers;
using Xunit;
using XUnitTestPizzAkuten.MockData;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestPizzAkuten
{
    public class DishTests
    {
    
        [Fact]
        public void Get_All_Nine_Dishes_Test()
        {

            var mockContext = DatabaseSetupTests.MockDataDbContext();
                var service = new DishService(mockContext, null);
                service.GetAllDishes();
            

                Assert.Equal(9, mockContext.Dishes.Count());
            
        }

        [Fact]
        public async Task Get_Dish_By_Id()
        {

            var mockContext = DatabaseSetupTests.MockDataDbContext();
            var controller = new DishController(null, null, mockContext);
            var result = await controller.Details(1);

            Assert.NotNull(result);

        }

        [Fact]
        public void  Get_Ingredients_NotNull()
        {

            var mockContext = DatabaseSetupTests.MockDataDbContext();
            var service = new DishService(mockContext, null);
            var result = service.GetAllIngredients();

            Assert.NotNull(result);

        }

        [Fact]
        public void Get_ExtraIngredients_NotNull()
        {

            var mockContext = DatabaseSetupTests.MockDataDbContext();
            var service = new DishService(mockContext, null);
            var result = service.GetAllExtraIngredients();

            Assert.NotNull(result);

        }

        [Fact]
        public void Add_New_Dish()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_Dish")
                .Options;
            var context = new ApplicationDbContext(options);
            var service = new DishService(context, null);

            var dish = new Dish
            {
                Name = "Gröt",
                Price = 49
            };
            context.Add(dish);
            context.SaveChanges();

            Assert.Equal("Gröt", context.Dishes.Single().Name);

        }

    }
}
