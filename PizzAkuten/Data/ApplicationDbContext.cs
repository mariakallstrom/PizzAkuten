using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzAkuten.Models;

namespace PizzAkuten.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredtients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
            {
            builder.Entity<DishIngredient>().HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);

            builder.Entity<OrderDish>().HasKey(od => new { od.DishId, od.OrderId });

            builder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.DishId);

            builder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.OrderId);

            base.OnModelCreating(builder);
                // Customize the ASP.NET Identity model and override the defaults if needed.
                // For example, you can rename the ASP.NET Identity table names and more.
                // Add your customizations after calling base.OnModelCreating(builder);
            }
        }
    }

