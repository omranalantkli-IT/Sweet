using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetFactory.Application.Contracts;
using SweetFactory.Application.Services;

namespace SweetFactory.Api.Controllers;

[ApiController, Route("api/work-entries"), Authorize]
public class WorkEntriesController(WorkEntryService entries) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<WorkEntryDto>> Create(CreateWorkEntryRequest request)
    {
        if (request.WorkDate > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest(new { message = "لا يمكن تسجيل تاريخ مستقبلي" });
        var workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var workerName = User.FindFirstValue(ClaimTypes.Name)!;
        var entry = await entries.CreateAsync(workerId, workerName, request);
        return entry is null ? BadRequest(new { message = "نوع العمل غير متاح" }) : Ok(entry);
    }

    [HttpGet]
    public Task<IReadOnlyList<WorkEntryDto>> Get([FromQuery] int? workerId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var currentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return entries.GetAsync(currentId, User.IsInRole("Admin"), workerId, from, to);
    }

    [Authorize(Roles = "Admin"), HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await entries.DeleteAsync(id) ? NoContent() : NotFound();
}
