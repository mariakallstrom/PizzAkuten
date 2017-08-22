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
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.se";
            aUser.Email = "student@test.se";
            var result = userManager.CreateAsync(aUser, "Passw0rd").Result;

            if (context.Dishes.ToList().Count == 0)
            {
                var pizza1 = new Dish
                {
                    Name = "Cappricciosa",
                    Price = 89
                };

                var pizza2 = new Dish { Name = "Margaritha", Price = 79 };
                var pizza3 = new Dish { Name = "Vesuvio", Price = 79 };
                context.AddRange(pizza1, pizza2, pizza3);
                context.SaveChanges();
            };



        }
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


                var cappricciosa = new Dish { Name = "Cappricciosa", Price = 89 };
                var margueritha = new Dish { Name = "Margaritha", Price = 79 };
                var vesuvio = new Dish { Name = "Vesuvio", Price = 79 };


                var capCheese = new DishIngredient { Dish = cappricciosa, Ingredient = cheese };
                var capTomatoe = new DishIngredient { Dish = cappricciosa, Ingredient = tomato };
                var capHam = new DishIngredient { Dish = cappricciosa, Ingredient = ham };
                var capMushroom = new DishIngredient { Dish = cappricciosa, Ingredient = mushroom };

                var marCheese = new DishIngredient { Dish = margueritha, Ingredient = cheese };
                var marTomatoe = new DishIngredient { Dish = margueritha, Ingredient = tomato };

                var vesCheese = new DishIngredient { Dish = vesuvio, Ingredient = cheese };
                var vesTomatoe = new DishIngredient { Dish = vesuvio, Ingredient = tomato };
                var vesHam = new DishIngredient { Dish = vesuvio, Ingredient = ham };

                cappricciosa.DishIngredients = new List<DishIngredient> {
                    capMushroom, capHam, capCheese, capTomatoe
                };

                vesuvio.DishIngredients = new List<DishIngredient> {
                    vesCheese, vesHam, vesTomatoe
                };

                margueritha.DishIngredients = new List<DishIngredient> {
                    marCheese, marTomatoe
                };


                context.AddRange(cappricciosa, margueritha, vesuvio);
                context.SaveChanges();
            };
        }
    }
}
