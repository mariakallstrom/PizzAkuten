﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzAkuten.Controllers;
using PizzAkuten.Data;
using PizzAkuten.Models;
using PizzAkuten.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using XUnitTestPizzAkuten.Fakes;

namespace XUnitTestPizzAkuten.FakeData
{
    public class BaseClassFakeData
    {

        public readonly IServiceProvider _serviceProvider;

        public BaseClassFakeData()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(x => x.UseInMemoryDatabase("PizzAkutenDb").UseInternalServiceProvider(serviceProvider));
            services.AddTransient<DishService>();
            services.AddTransient<OrderService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<DishController>();
            services.AddTransient<HomeController>();
            services.AddTransient<OrderController>();
            services.AddTransient<IHostingEnvironment, FakeHostingEnviroment>();
            services.AddTransient<ISession, FakeSession>();
            services.AddTransient<IUserStore<ApplicationUser>, FakeUserStore>();
            services.AddTransient<IPasswordHasher<ApplicationUser>, FakePasswordHasher>();
            services.AddTransient<ILookupNormalizer, FakeLookupNormailizer>();
            services.AddTransient<IdentityErrorDescriber>();
            services.AddTransient<UserManager<ApplicationUser>, FakeUserManager>();
            services.AddTransient<IHttpContextAccessor, FakeContextAccessor>();
            //services.AddTransient<IEmailSender, TestEmaiLSender>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public virtual void FakeDataInitializer()
        {
            var context = _serviceProvider.GetService<ApplicationDbContext>();

            var cheese = new Ingredient { Name = "Cheese", Price = 5 };
            var ham = new Ingredient { Name = "Ham", Price = 5 };
            var tomato = new Ingredient { Name = "Tomato", Price = 5 };
            var mushroom = new Ingredient { Name = "Mushroom", Price = 5 };
            var kebab = new Ingredient { Name = "Kebab", Price = 10 };
            var pasta = new Ingredient { Name = "Pasta", Price = 5 };
            var tuna = new Ingredient { Name = "Tonfisk", Price = 5 };
            var salad = new Ingredient { Name = "Sallad", Price = 5 };
            var chicken = new Ingredient { Name = "Chicken", Price = 10 };
            var pork = new Ingredient { Name = "Fläskfilé", Price = 10 };
            var beef = new Ingredient { Name = "Oxfilé", Price = 10 };
            var cucumber = new Ingredient { Name = "Gurka", Price = 5 };
            var paprika = new Ingredient { Name = "Paprika", Price = 5 };
            var ananas = new Ingredient { Name = "Ananas", Price = 5 };
            var banana = new Ingredient { Name = "Banana", Price = 5 };
            var bread = new Ingredient { Name = "Bröd", Price = 5 };
            var potatoe = new Ingredient { Name = "Potatis", Price = 15 };
            var dressing = new Ingredient { Name = "Dressing", Price = 10 };
            var onion = new Ingredient { Name = "Lök", Price = 5 };
            context.AddRange(cheese, ham, tomato, mushroom, kebab, pasta, tuna, salad, chicken, pork, beef, cucumber, paprika, ananas, banana, bread, potatoe, dressing, onion);
            context.SaveChanges();

            if (context.Dishes.ToList().Count == 0)
            {
                var capricciosa = new Dish { Name = "Cappricciosa", Price = 89, ImagePath = "/images/pizza.jpg" };
                var margueritha = new Dish { Name = "Margaritha", Price = 79, ImagePath = "/images/pizza.jpg" };
                var vesuvio = new Dish { Name = "Vesuvio", Price = 79, ImagePath = "/images/pizza.jpg" };
                var pastaPork = new Dish { Name = "Pasta med Fläskfilé", Price = 89, ImagePath = "/images/pasta.jpg" };
                var pastaBeef = new Dish { Name = "Pasta med Oxfilé", Price = 99, ImagePath = "/images/pasta.jpg" };
                var chickenSalad = new Dish { Name = "KycklingSallad", Price = 79, ImagePath = "/images/sallad.jpg" };
                var kebabSalad = new Dish { Name = "Kebabsallad", Price = 79, ImagePath = "/images/sallad.jpg" };
                var kebabDish = new Dish { Name = "KebabTallrik", Price = 89, ImagePath = "/images/kebab.jpg" };
                var hamburger = new Dish { Name = "Hamburgare 90g", Price = 89, ImagePath = "/images/hamburgare.jpg" };

                capricciosa.DishIngredients = new List<DishIngredient> {
                    new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = ham},
                    new DishIngredient{Ingredient = mushroom}
                };

                vesuvio.DishIngredients = new List<DishIngredient> {
                    new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = ham},
                };

                margueritha.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                };

                chickenSalad.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = chicken},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                       new DishIngredient{Ingredient = onion},
                };

                kebabSalad.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = kebab},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                       new DishIngredient{Ingredient = onion},
                };

                pastaBeef.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = beef},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                       new DishIngredient{Ingredient = onion},
                };

                pastaPork.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = pork},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                       new DishIngredient{Ingredient = onion},
                };

                kebabDish.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = kebab},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                        new DishIngredient{Ingredient = potatoe},
                         new DishIngredient{Ingredient = onion},
                };

                hamburger.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = beef},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                        new DishIngredient{Ingredient = potatoe},
                         new DishIngredient{Ingredient = dressing},
                          new DishIngredient{Ingredient = onion},
                };



                var cpizza = new Category { Name = "Pizza" };
                var csalad = new Category { Name = "Sallad" };
                var cpasta = new Category { Name = "Pasta" };
                var cother = new Category { Name = "Övrigt" };


                capricciosa.Category = cpizza;
                vesuvio.Category = cpizza;
                margueritha.Category = cpizza;
                chickenSalad.Category = csalad;
                kebabSalad.Category = csalad;
                pastaBeef.Category = cpasta;
                pastaPork.Category = cpasta;
                hamburger.Category = cother;
                kebabDish.Category = cother;

                context.AddRange(capricciosa, margueritha, vesuvio, pastaBeef, pastaPork, kebabSalad, kebabDish, chickenSalad, hamburger);
                context.SaveChanges();

            };
            if (context.ExtraIngredients.ToList().Count == 0)
            {
                var xcheese = new ExtraIngredient { Name = "Cheese", Price = 5 };
                var xham = new ExtraIngredient { Name = "Ham", Price = 5 };
                var xtomato = new ExtraIngredient { Name = "Tomato", Price = 5 };
                var xmushroom = new ExtraIngredient { Name = "Mushroom", Price = 5 };
                var xkebab = new ExtraIngredient { Name = "Kebab", Price = 10 };
                var xpasta = new ExtraIngredient { Name = "Pasta", Price = 5 };
                var xtuna = new ExtraIngredient { Name = "Tonfisk", Price = 5 };
                var xsalad = new ExtraIngredient { Name = "Sallad", Price = 5 };
                var xchicken = new ExtraIngredient { Name = "Chicken", Price = 10 };
                var xpork = new ExtraIngredient { Name = "Fläskfilé", Price = 10 };
                var xbeef = new ExtraIngredient { Name = "Oxfilé", Price = 10 };
                var xcucumber = new ExtraIngredient { Name = "Gurka", Price = 5 };
                var xpaprika = new ExtraIngredient { Name = "Paprika", Price = 5 };
                var xananas = new ExtraIngredient { Name = "Ananas", Price = 5 };
                var xbanana = new ExtraIngredient { Name = "Banana", Price = 5 };
                var xbread = new ExtraIngredient { Name = "Bröd", Price = 5 };
                var xpotatoe = new ExtraIngredient { Name = "Potatis", Price = 15 };
                var xdressing = new ExtraIngredient { Name = "Dressing", Price = 10 };
                var xonion = new ExtraIngredient { Name = "Lök", Price = 5 };

                context.AddRange(xcheese, xham, xtomato, xmushroom, xkebab, xpasta, xtuna, xsalad, xchicken, xpork, xbeef, xcucumber, xpaprika, xananas, xbanana, xbread, xpotatoe, xdressing, xonion);
                context.SaveChanges();



            };
        }
    }
}
