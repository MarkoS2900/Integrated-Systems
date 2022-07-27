using Cinema.Domain.DomainModels;
using Cinema.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cinema.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>().Property(z => z.Id).ValueGeneratedOnAdd();

            builder.Entity<ProductInCart>().HasOne(z => z.product).WithMany(z => z.productsInCart)
                .HasForeignKey(z => z.shoppingCartID);

            builder.Entity<ProductInCart>().HasOne(z => z.shoppingCart).WithMany(z => z.productsInCart)
                .HasForeignKey(z => z.productId);

            builder.Entity<ShoppingCart>().HasOne<ApplicationUser>(z => z.owner).WithOne(z => z.userCart)
                .HasForeignKey<ShoppingCart>(z => z.ownerId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.product)
                .WithMany(z => z.productInOrders)
                .HasForeignKey(z => z.productId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.order)
                .WithMany(z => z.productInOrders)
                .HasForeignKey(z => z.orderId);
        }
    }
}
