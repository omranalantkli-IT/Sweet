using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Models;

namespace SweetFactory.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IConfiguration configuration)
    {
        if (!await db.Users.AnyAsync())
        {
            var admin = new User
            {
                FullName = configuration["SeedAdmin:FullName"] ?? "مدير المعمل",
                Username = configuration["SeedAdmin:Username"] ?? "admin",
                PasswordHash = string.Empty,
                Role = "Admin"
            };
            admin.PasswordHash = new PasswordHasher<User>().HashPassword(
                admin, configuration["SeedAdmin:Password"] ?? "Admin@123");
            db.Users.Add(admin);
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product { Name = "بقلاوة", UnitName = "صينية", RatePerUnit = 15 },
                new Product { Name = "كنافة", UnitName = "صينية", RatePerUnit = 12 },
                new Product { Name = "معمول", UnitName = "كيلو", RatePerUnit = 8 });
        }
        await db.SaveChangesAsync();
    }
}
