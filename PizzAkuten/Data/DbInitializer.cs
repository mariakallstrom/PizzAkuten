﻿using Microsoft.AspNetCore.Identity;
using PizzAkuten.Extensions;
using PizzAkuten.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzAkuten.Data
{
    public class DbInitializer
    {
     
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "Student";
            aUser.Email = "student@test.se";
            var result = userManager.CreateAsync(aUser, "Passw0rd").Result;

            if (result.Succeeded)
            {
                var adminRole = new IdentityRole("member");
                var roleResult = roleManager.CreateAsync(adminRole).Result;
                userManager.AddToRoleAsync(aUser, adminRole.Name);
            }

            var adminUser = new ApplicationUser();
            adminUser.UserName = "Admin";
            adminUser.Email = "admin@test.se";
            var adminResult = userManager.CreateAsync(adminUser, "Admin0").Result;

            if (adminResult.Succeeded)
            {
                var adminRole = new IdentityRole("admin");
                var roleResult = roleManager.CreateAsync(adminRole).Result;
                userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }

            if (context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingredient { Name = "Cheese", Price=5 };
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


                var capricciosa = new Dish { Name = "Cappricciosa", Price = 89, ImagePath = "images/pizza.jpg"};
                var margueritha = new Dish { Name = "Margaritha", Price = 79, ImagePath = "images/pizza.jpg" };
                var vesuvio = new Dish { Name = "Vesuvio", Price = 79, ImagePath = "images/pizza.jpg" };
                var pastaPork = new Dish { Name = "Pasta med Fläskfilé", Price = 89, ImagePath = "images/pasta.jpg" };
                var pastaBeef = new Dish { Name = "Pasta med Oxfilé", Price = 99, ImagePath = "images/pasta.jpg" };
                var chickenSalad = new Dish { Name = "KycklingSallad", Price = 79, ImagePath = "images/sallad.jpg" };
                var kebabSalad = new Dish { Name = "Kebabsallad", Price = 79, ImagePath = "images/sallad.jpg" };
                var kebabDish = new Dish { Name = "KebabTallrik", Price = 89, ImagePath = "images/kebab.jpg" };
                var hamburger = new Dish { Name = "Hamburgare 90g m. Pommes", Price = 89, ImagePath = "images/hamburgare.jpg" };

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
            


                var cpizza = new Category { CategoryId = 1, Name = "Pizza" };
                var csalad = new Category { CategoryId = 2, Name = "Sallad" };
                var cpasta = new Category { CategoryId = 3, Name = "Pasta" };
                var cother = new Category { CategoryId = 4, Name = "Övrigt" };
               

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
        }
    }
}
