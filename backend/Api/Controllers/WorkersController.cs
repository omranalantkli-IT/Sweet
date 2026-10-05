using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetFactory.Application.Services;
using SweetFactory.Application.Contracts;

namespace SweetFactory.Api.Controllers;

[ApiController, Route("api/workers"), Authorize(Roles = "Admin")]
public class WorkersController(WorkerService workers) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<UserDto>> Get() => workers.GetAsync();

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateWorkerRequest request)
    {
        var worker = await workers.CreateAsync(request);
        return worker is null ? Conflict(new { message = "اسم المستخدم مستخدم مسبقًا" }) : CreatedAtAction(nameof(Get), worker);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateWorkerRequest request)
    {
        return await workers.UpdateAsync(id, request) ? NoContent() : NotFound();
    }
}
