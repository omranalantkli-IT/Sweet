using Microsoft.EntityFrameworkCore;
using SweetFactory.Application.Contracts;
using SweetFactory.Domain.Entities;
using SweetFactory.Infrastructure.Persistence;

namespace SweetFactory.Application.Services;

public class ProductService(AppDbContext db)
{
    public async Task<IReadOnlyList<ProductDto>> GetAsync() => await db.Products.AsNoTracking()
        .OrderBy(product => product.Name)
        .Select(product => new ProductDto(product.Id, product.Name, product.UnitName, product.RatePerUnit, product.IsActive))
        .ToListAsync();

    public async Task<ProductDto?> CreateAsync(SaveProductRequest request)
    {
        var name = request.Name.Trim();
        if (await db.Products.AnyAsync(product => product.Name == name)) return null;
        var product = new Product
        {
            Name = name,
            UnitName = request.UnitName.Trim(),
            RatePerUnit = request.RatePerUnit,
            IsActive = request.IsActive
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return new ProductDto(product.Id, product.Name, product.UnitName, product.RatePerUnit, product.IsActive);
    }

    public async Task<bool> UpdateAsync(int id, SaveProductRequest request)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return false;
        product.Name = request.Name.Trim();
        product.UnitName = request.UnitName.Trim();
        product.RatePerUnit = request.RatePerUnit;
        product.IsActive = request.IsActive;
        await db.SaveChangesAsync();
        return true;
    }
}
