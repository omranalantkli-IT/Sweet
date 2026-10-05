using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Data;
using SweetFactory.Dtos;
using SweetFactory.Models;

namespace SweetFactory.Controllers;

[ApiController, Route("api/work-entries"), Authorize]
public class WorkEntriesController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<WorkEntryDto>> Create(CreateWorkEntryRequest request)
    {
        var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var product = await db.Products.FindAsync(request.ProductId);
        if (product is null || !product.IsActive) return BadRequest(new { message = "نوع العمل غير متاح" });
        if (request.WorkDate > DateOnly.FromDateTime(DateTime.Today)) return BadRequest(new { message = "لا يمكن تسجيل تاريخ مستقبلي" });
        var entry = new WorkEntry { WorkerId = workerId, ProductId = product.Id, WorkDate = request.WorkDate,
            Quantity = request.Quantity, UnitRateSnapshot = product.RatePerUnit, Notes = request.Notes?.Trim() };
        db.WorkEntries.Add(entry); await db.SaveChangesAsync();
        var workerName = User.FindFirstValue(ClaimTypes.Name)!;
        return Ok(ToDto(entry, workerName, product));
    }

    [HttpGet]
    public async Task<IReadOnlyList<WorkEntryDto>> Get([FromQuery] int? workerId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");
        var query = db.WorkEntries.AsNoTracking().Include(x => x.Worker).Include(x => x.Product).AsQueryable();
        query = isAdmin && workerId.HasValue ? query.Where(x => x.WorkerId == workerId) : isAdmin ? query : query.Where(x => x.WorkerId == currentId);
        if (from.HasValue) query = query.Where(x => x.WorkDate >= from.Value);
        if (to.HasValue) query = query.Where(x => x.WorkDate <= to.Value);
        return await query.OrderByDescending(x => x.WorkDate).ThenByDescending(x => x.Id)
            .Select(x => new WorkEntryDto(x.Id, x.WorkerId, x.Worker.FullName, x.ProductId, x.Product.Name, x.Product.UnitName,
                x.WorkDate, x.Quantity, x.UnitRateSnapshot, x.Quantity * x.UnitRateSnapshot, x.Notes)).ToListAsync();
    }

    [Authorize(Roles = "Admin"), HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    { var entry = await db.WorkEntries.FindAsync(id); if (entry is null) return NotFound(); db.Remove(entry); await db.SaveChangesAsync(); return NoContent(); }

    private static WorkEntryDto ToDto(WorkEntry x, string workerName, Product p) =>
        new(x.Id, x.WorkerId, workerName, p.Id, p.Name, p.UnitName, x.WorkDate, x.Quantity, x.UnitRateSnapshot, x.TotalAmount, x.Notes);
}
