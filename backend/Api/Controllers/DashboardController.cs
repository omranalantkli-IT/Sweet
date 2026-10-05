using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetFactory.Application.Contracts;
using SweetFactory.Application.Services;

namespace SweetFactory.Api.Controllers;

[ApiController, Route("api/dashboard"), Authorize(Roles = "Admin")]
public class DashboardController(DashboardService dashboard) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AdminDashboardDto>> Get() => Ok(await dashboard.GetAsync());

    [HttpGet("monthly-summary")]
    public async Task<ActionResult<IReadOnlyList<MonthlyWorkerSummary>>> MonthlySummary([FromQuery] int year, [FromQuery] int month)
    {
        if (year is < 2020 or > 2100 || month is < 1 or > 12)
            return BadRequest(new { message = "الفترة المحددة غير صالحة" });
        return Ok(await dashboard.GetMonthlySummaryAsync(year, month));
    }
}
