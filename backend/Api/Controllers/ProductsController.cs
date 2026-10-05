using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetFactory.Application.Services;
using SweetFactory.Application.Contracts;

namespace SweetFactory.Api.Controllers;

[ApiController, Route("api/products"), Authorize]
public class ProductsController(ProductService products) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<ProductDto>> Get() => products.GetAsync();

    [Authorize(Roles = "Admin"), HttpPost]
    public async Task<ActionResult<ProductDto>> Create(SaveProductRequest request)
    {
        var product = await products.CreateAsync(request);
        return product is null ? Conflict(new { message = "نوع العمل موجود مسبقًا" }) : Ok(product);
    }

    [Authorize(Roles = "Admin"), HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SaveProductRequest request)
    {
        return await products.UpdateAsync(id, request) ? NoContent() : NotFound();
    }
}
