using System;
using AutoLotDALCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLotDALCore.EF
{
    public class AutoLotContext : DbContext
    {
        public AutoLotContext(DbContextOptions options) : base(options) { }

        internal AutoLotContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=(localdb)\MSSQLLocalDB;database=AutoLotCore;
                                       integrated security=True;MultipleActiveResultSets=True;
                                       App=EntityFramework;";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }

        public DbSet<CreditRisk> CreditRisks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasIndex(e => new {e.FirstName, e.LastName}).IsUnique();
            });
            modelBuilder.Entity<Order>()
                .HasOne(e => e.Car)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).GetTableName();
        }
    }
}