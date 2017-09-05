using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzAkuten.Controllers;
using PizzAkuten.Data;
using System;
using System.Linq;
using Xunit;

namespace XUnitTestPizzAkuten
{
    public class HomeControllerTest
    {
        private readonly IServiceProvider _serviceProvider;
        public HomeControllerTest()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b => b.UseInMemoryDatabase("Default").UseInternalServiceProvider(efServiceProvider));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void ReturnsIndexView()
        {
            var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            // Arrange
            var controller = new HomeController(dbContext);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.ViewName);
        }
    }
}
