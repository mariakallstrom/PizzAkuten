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
    }
}
