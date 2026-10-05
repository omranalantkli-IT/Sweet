using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Domain.Entities;

namespace SweetFactory.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IConfiguration configuration)
    {
        var adminUsername = configuration["SeedAdmin:Username"] ?? "admin";
        var adminPassword = configuration["SeedAdmin:Password"] ?? "Admin@123";
        var adminFullName = configuration["SeedAdmin:FullName"] ?? "مدير المعمل";

        var admin = await db.Users.FirstOrDefaultAsync(x => x.Username == adminUsername);

        if (admin is null)
        {
            admin = new User
            {
                FullName = adminFullName,
                Username = adminUsername,
                PasswordHash = string.Empty,
                Role = "Admin"
            };

            admin.PasswordHash = new PasswordHasher<User>().HashPassword(
                admin,
                adminPassword);

            db.Users.Add(admin);
        }
        else
        {
            admin.FullName = adminFullName;
            admin.Role = "Admin";
            admin.PasswordHash = new PasswordHasher<User>().HashPassword(
                admin,
                adminPassword);
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
