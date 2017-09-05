using Microsoft.AspNetCore.Identity;
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
            aUser.UserName = "student@test.se";
            aUser.Email = "student@test.se";
            aUser.City = "Ankeborg";
            aUser.ZipCode = "11313";
            aUser.Street = "Ankgatan 1";
            aUser.FirstName = "Kalle";
            aUser.LastName = "Anka";
            var result = userManager.CreateAsync(aUser, "Passw0rd").Result;

            if (result.Succeeded)
            {
                var adminRole = new IdentityRole("member");
                var roleResult = roleManager.CreateAsync(adminRole).Result;
                userManager.AddToRoleAsync(aUser, adminRole.Name);
            }

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.se";
            adminUser.Email = "admin@test.se";
            var adminResult = userManager.CreateAsync(adminUser, "Admin0").Result;

            if (adminResult.Succeeded)
            {
                var adminRole = new IdentityRole("admin");
                var roleResult = roleManager.CreateAsync(adminRole).Result;
                userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
         
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
                var capricciosa = new Dish { Name = "Cappricciosa", Price = 89, ImagePath = "/images/pizza.jpg"};
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
            


                var cpizza = new Category {Name = "Pizza" };
                var csalad = new Category {Name = "Sallad" };
                var cpasta = new Category {Name = "Pasta" };
                var cother = new Category {Name = "Övrigt" };
               

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

                var payment = new Payment { CardNumber = "123", ApplicationUser = aUser, Cvv = 123, Month = 10, Year = 2019, PayMethod = "Visa" };
                var order = new Order();
                order.ApplicationUser = aUser;
                order.Cart = new Cart { CartItems = new List<CartItem> { new CartItem { Dish = capricciosa, Quantity = 1 } }, TotalPrice = capricciosa.Price };
                order.TotalPrice = capricciosa.Price;
                order.Payment = payment;
                order.OrderDate = DateTime.Now;
                payment.Order = order;
                order.Payment = payment;

                var payment2 = new Payment { CardNumber = "123", ApplicationUser = aUser, Cvv = 123, Month = 10, Year = 2019, PayMethod = "Visa" };
                var order2 = new Order();
                order2.ApplicationUser = aUser;
                order2.Cart = new Cart { CartItems = new List<CartItem> { new CartItem { Dish = kebabDish, Quantity = 1 } }, TotalPrice = kebabDish.Price };
                order2.TotalPrice = kebabDish.Price;
                order2.Payment = payment;
                order2.OrderDate = DateTime.Now;
                payment2.Order = order2;
                order2.Payment = payment2;


                context.Orders.AddRange(order, order2);
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
