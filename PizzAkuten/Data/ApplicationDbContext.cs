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
        public DbSet<ExtraIngredient> ExtraIngredients { get; set; }
        public DbSet<DishExtraIngredient> DishExtraIngredients { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<NonAccountUser> NonAccountUsers { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


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

            builder.Entity<DishExtraIngredient>().HasKey(di => new { di.DishId, di.ExtraIngredientId });

            builder.Entity<DishExtraIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishExtraIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishExtraIngredient>()
                .HasOne(di => di.ExtraIngredient)
                .WithMany(i => i.DishExtraIngredients)
                .HasForeignKey(di => di.ExtraIngredientId);

            builder.Entity<Cart>().HasKey(od => od.OrderId);

            base.OnModelCreating(builder);
                // Customize the ASP.NET Identity model and override the defaults if needed.
                // For example, you can rename the ASP.NET Identity table names and more.
                // Add your customizations after calling base.OnModelCreating(builder);
            }

       
        }
    }

