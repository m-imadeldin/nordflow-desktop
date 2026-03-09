// ========================================
// DATABASE CONTEXT - EF Core entry point
// ========================================
using Microsoft.EntityFrameworkCore;
using NordFlow.Models;

namespace NordFlow.Data
{
    public class NordFlowDbContext : DbContext
    {
        // Database tables (DbSet)
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        // ========================================
        // DATABASE CONFIGURATION
        // ========================================
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQLite with NordFlow.db file in app directory
            optionsBuilder.UseSqlite("Data Source=NordFlow.db");
        }

        // ========================================
        // ENTITY RELATIONSHIPS
        // ========================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer has many Invoices (1:N relationship)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerId);
        }
    }
} 