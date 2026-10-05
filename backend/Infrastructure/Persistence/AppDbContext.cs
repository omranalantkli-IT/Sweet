using Microsoft.EntityFrameworkCore;
using SweetFactory.Domain.Entities;

namespace SweetFactory.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<WorkEntry> WorkEntries => Set<WorkEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(x => x.FullName).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(x => x.Role).HasMaxLength(20);
        modelBuilder.Entity<Product>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(x => x.UnitName).HasMaxLength(30);
        modelBuilder.Entity<WorkEntry>().Property(x => x.Notes).HasMaxLength(500);
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<WorkEntry>().HasIndex(x => new { x.WorkerId, x.WorkDate });
        modelBuilder.Entity<WorkEntry>().Property(x => x.Quantity).HasPrecision(12, 2);
        modelBuilder.Entity<WorkEntry>().Property(x => x.UnitRateSnapshot).HasPrecision(12, 2);
        modelBuilder.Entity<Product>().Property(x => x.RatePerUnit).HasPrecision(12, 2);
        modelBuilder.Entity<WorkEntry>().HasOne(x => x.Worker).WithMany(x => x.WorkEntries)
            .HasForeignKey(x => x.WorkerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<WorkEntry>().HasOne(x => x.Product).WithMany(x => x.WorkEntries)
            .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}
