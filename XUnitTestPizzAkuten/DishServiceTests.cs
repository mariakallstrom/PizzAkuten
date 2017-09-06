using Microsoft.Extensions.DependencyInjection;
using PizzAkuten.Data;
using PizzAkuten.Services;
using System.Linq;
using Xunit;
using XUnitTestPizzAkuten.FakeData;

namespace XUnitTestPizzAkuten
{
    public class DishServiceTests : BaseClassFakeData
    {
        public DishServiceTests()
        {
            base.FakeDataInitializer();
        }
    
        [Fact]
        public void Get_All_Dishes_NotNull()
        {
            var service = _serviceProvider.GetService<DishService>();
            var allDishes = service.GetAllDishes();
            Assert.NotNull(allDishes);
        }

        [Fact]
        public void Get_Dish_By_Id_NotNull()
        {
            var context = _serviceProvider.GetService<ApplicationDbContext>();
            var getDish = context.Dishes.Find(1);
            Assert.NotNull(getDish);
        }

        [Fact]
        public void  Get_All_Ingredients_NotNull()
        {
            var service = _serviceProvider.GetService<DishService>();
            var ingredients = service.GetAllIngredients();
            Assert.NotNull(ingredients);
        }

        [Fact]
        public void Get_All_ExtraIngredients_NotNull()
        {
            var xdishes = _serviceProvider.GetService<DishService>();
            var xingredients = xdishes.GetAllExtraIngredients();
            Assert.NotNull(xingredients);
        }

        [Fact]
        public void Ingredients_Are_Sorted()
        {
            var service = _serviceProvider.GetService<DishService>();
            var ingredients = service.GetAllIngredients();
            var expectedList = ingredients.OrderBy(x => x.Name);
            Assert.True(expectedList.SequenceEqual(ingredients));
        }
        [Fact]
        public void ExtraIngredients_Are_Sorted()
        {
            var service = _serviceProvider.GetService<DishService>();
            var xingredients = service.GetAllExtraIngredients();
            var expectedList = xingredients.OrderBy(x => x.Name);
            Assert.True(expectedList.SequenceEqual(xingredients));
        }
    }
}
