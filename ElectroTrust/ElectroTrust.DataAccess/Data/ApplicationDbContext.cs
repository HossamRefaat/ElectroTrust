using ElectroTrust.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroTrust.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customization> Customizations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Customer (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            // User -> Seller (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Seller)
                .WithOne(s => s.User)
                .HasForeignKey<Seller>(s => s.SellerID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            // Seller -> Products (One-to-Many)
            modelBuilder.Entity<Seller>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Category -> Products (One-to-Many)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer -> Orders (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            // Order -> OrderItems (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem -> Customization (Optional One-to-One)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Customization)
                .WithOne()
                .HasForeignKey<OrderItem>(oi => oi.CustomizationID)
                .OnDelete(DeleteBehavior.SetNull); // Allow null if customization is deleted

            // Order -> Payment (One-to-One)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

            // Customer -> Reviews (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Reviews (One-to-Many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer -> Wishlist (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Wishlists)
                .WithOne(w => w.Customer)
                .HasForeignKey(w => w.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Wishlist (One-to-Many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Wishlists)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer -> Customizations (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Customizations)
                .WithOne(cz => cz.Customer)
                .HasForeignKey(cz => cz.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Customizations (One-to-Many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Customizations)
                .WithOne(cz => cz.Product)
                .HasForeignKey(cz => cz.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
