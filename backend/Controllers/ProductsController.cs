using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Data;
using SweetFactory.Dtos;
using SweetFactory.Models;

namespace SweetFactory.Controllers;

[ApiController, Route("api/products"), Authorize]
public class ProductsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<ProductDto>> Get() => await db.Products.OrderBy(x => x.Name)
        .Select(x => new ProductDto(x.Id, x.Name, x.UnitName, x.RatePerUnit, x.IsActive)).ToListAsync();

    [Authorize(Roles = "Admin"), HttpPost]
    public async Task<ActionResult<ProductDto>> Create(SaveProductRequest request)
    {
        if (await db.Products.AnyAsync(x => x.Name == request.Name.Trim())) return Conflict(new { message = "نوع العمل موجود مسبقًا" });
        var item = new Product { Name = request.Name.Trim(), UnitName = request.UnitName.Trim(), RatePerUnit = request.RatePerUnit, IsActive = request.IsActive };
        db.Products.Add(item); await db.SaveChangesAsync();
        return Ok(new ProductDto(item.Id, item.Name, item.UnitName, item.RatePerUnit, item.IsActive));
    }

    [Authorize(Roles = "Admin"), HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SaveProductRequest request)
    {
        var item = await db.Products.FindAsync(id); if (item is null) return NotFound();
        item.Name = request.Name.Trim(); item.UnitName = request.UnitName.Trim(); item.RatePerUnit = request.RatePerUnit; item.IsActive = request.IsActive;
        await db.SaveChangesAsync(); return NoContent();
    }
}
