using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Data;
using SweetFactory.Dtos;

namespace SweetFactory.Controllers;

[ApiController, Route("api/dashboard"), Authorize(Roles = "Admin")]
public class DashboardController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AdminDashboardDto>> Get()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var activeWorkers = await db.Users.CountAsync(x => x.Role == "Worker" && x.IsActive);
        var todayQuantity = await db.WorkEntries.Where(x => x.WorkDate == today).SumAsync(x => (decimal?)x.Quantity) ?? 0;
        var monthQuery = db.WorkEntries.Where(x => x.WorkDate >= monthStart && x.WorkDate <= today);
        var monthTotal = await monthQuery.SumAsync(x => (decimal?)(x.Quantity * x.UnitRateSnapshot)) ?? 0;
        var count = await monthQuery.CountAsync();
        var recent = await db.WorkEntries.Include(x => x.Worker).Include(x => x.Product).OrderByDescending(x => x.Id).Take(8)
            .Select(x => new WorkEntryDto(x.Id, x.WorkerId, x.Worker.FullName, x.ProductId, x.Product.Name, x.Product.UnitName,
                x.WorkDate, x.Quantity, x.UnitRateSnapshot, x.Quantity * x.UnitRateSnapshot, x.Notes)).ToListAsync();
        return Ok(new AdminDashboardDto(activeWorkers, todayQuantity, monthTotal, count, recent));
    }

    [HttpGet("monthly-summary")]
    public async Task<ActionResult<IReadOnlyList<MonthlyWorkerSummary>>> MonthlySummary([FromQuery] int year, [FromQuery] int month)
    {
        if (year is < 2020 or > 2100 || month is < 1 or > 12)
            return BadRequest(new { message = "الفترة المحددة غير صالحة" });
        var start = new DateOnly(year, month, 1); var end = start.AddMonths(1);
        var entries = await db.WorkEntries.AsNoTracking()
            .Where(x => x.WorkDate >= start && x.WorkDate < end)
            .Select(x => new { x.WorkerId, x.Worker.FullName, x.Quantity, x.UnitRateSnapshot })
            .ToListAsync();
        var summary = entries.GroupBy(x => new { x.WorkerId, x.FullName })
            .Select(g => new MonthlyWorkerSummary(g.Key.WorkerId, g.Key.FullName,
                g.Sum(x => x.Quantity), g.Sum(x => x.Quantity * x.UnitRateSnapshot)))
            .OrderBy(x => x.WorkerName).ToList();
        return Ok(summary);
    }
}
