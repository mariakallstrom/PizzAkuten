using Microsoft.AspNetCore.Identity;
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
                var cheese = new Ingredient { Name = "Cheese" };
                var ham = new Ingredient { Name = "Ham" };
                var tomato = new Ingredient { Name = "Tomato" };
                var mushroom = new Ingredient { Name = "Mushroom" };
                var kebab = new Ingredient { Name = "Kebab" };
                var pasta = new Ingredient { Name = "Pasta" };
                var tuna = new Ingredient { Name = "Tonfisk" };
                var salad = new Ingredient { Name = "Sallad" };
                var chicken = new Ingredient { Name = "Chicken" };
                var pork = new Ingredient { Name = "Fläskfilé" };
                var beef = new Ingredient { Name = "Oxfilé" };
                var cucumber = new Ingredient { Name = "Gurka" };
                var paprika = new Ingredient { Name = "Paprika" };
                var ananas = new Ingredient { Name = "Ananas" };
                var banana = new Ingredient { Name = "Banana" };
                var bread = new Ingredient { Name = "Bröd" };
                var potatoe = new Ingredient { Name = "Potatis" };
                var dressing = new Ingredient { Name = "Dressing" };


                var cappricciosa = new Dish { Name = "Cappricciosa", Price = 89 };
                var margueritha = new Dish { Name = "Margaritha", Price = 79 };
                var vesuvio = new Dish { Name = "Vesuvio", Price = 79 };
                var pastaPork = new Dish { Name = "Pasta med Fläskfilé", Price = 89 };
                var pastaBeef = new Dish { Name = "Pasta med Oxfilé", Price = 99 };
                var chickenSalad = new Dish { Name = "KycklingSallad", Price = 79 };
                var kebabSalad = new Dish { Name = "Kebabsallad", Price = 79 };
                var kebabDish = new Dish { Name = "KebabTallrik", Price = 89 };
                var hamburger = new Dish { Name = "Hamburgare 90g m. Pommes", Price = 89 };

                //var capCheese = new DishIngredient { Dish = cappricciosa, Ingredient = cheese };
                //var capTomatoe = new DishIngredient { Dish = cappricciosa, Ingredient = tomato };
                //var capHam = new DishIngredient { Dish = cappricciosa, Ingredient = ham };
                //var capMushroom = new DishIngredient { Dish = cappricciosa, Ingredient = mushroom };

                //var marCheese = new DishIngredient { Dish = margueritha, Ingredient = cheese };
                //var marTomatoe = new DishIngredient { Dish = margueritha, Ingredient = tomato };

                //var vesCheese = new DishIngredient { Dish = vesuvio, Ingredient = cheese };
                //var vesTomatoe = new DishIngredient { Dish = vesuvio, Ingredient = tomato };
                //var vesHam = new DishIngredient { Dish = vesuvio, Ingredient = ham };

                cappricciosa.DishIngredients = new List<DishIngredient> {
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
                };

                kebabSalad.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = kebab},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                };

                pastaBeef.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = beef},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                };

                pastaPork.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = pork},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                };

                kebabDish.DishIngredients = new List<DishIngredient> {
                     new DishIngredient{Ingredient = cheese},
                    new DishIngredient{Ingredient = tomato},
                    new DishIngredient{Ingredient = salad},
                    new DishIngredient{Ingredient = kebab},
                    new DishIngredient{Ingredient = cucumber},
                       new DishIngredient{Ingredient = paprika},
                        new DishIngredient{Ingredient = potatoe},
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
                };

                var categoryList = new List<Category>
                {
                    new Category{CategoryId=1, Name="Pizza"},
                    new Category{CategoryId=2, Name="Sallad"},
                    new Category{CategoryId=3, Name="Pasta"},
                    new Category{CategoryId=4, Name="Övrigt"}
                };

                cappricciosa.CategoryId = 1;
                vesuvio.CategoryId = 1;
                margueritha.CategoryId = 1;
                chickenSalad.CategoryId = 2;
                kebabSalad.CategoryId = 2;
                pastaBeef.CategoryId = 3;
                pastaPork.CategoryId = 3;
                hamburger.CategoryId = 4;

                context.AddRange(cappricciosa, margueritha, vesuvio, pastaBeef, pastaPork, kebabSalad, kebabDish, chickenSalad, hamburger);
                context.SaveChanges();
            };
        }
    }
}
