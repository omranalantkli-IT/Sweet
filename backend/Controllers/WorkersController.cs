using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Data;
using SweetFactory.Dtos;
using SweetFactory.Models;

namespace SweetFactory.Controllers;

[ApiController, Route("api/workers"), Authorize(Roles = "Admin")]
public class WorkersController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<UserDto>> Get() => await db.Users.Where(x => x.Role == "Worker")
        .OrderBy(x => x.FullName).Select(x => new UserDto(x.Id, x.FullName, x.Username, x.Role, x.IsActive)).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateWorkerRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Username == username)) return Conflict(new { message = "اسم المستخدم مستخدم مسبقًا" });
        var worker = new User { FullName = request.FullName.Trim(), Username = username, PasswordHash = "", Role = "Worker" };
        worker.PasswordHash = new PasswordHasher<User>().HashPassword(worker, request.Password);
        db.Users.Add(worker); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new UserDto(worker.Id, worker.FullName, worker.Username, worker.Role, worker.IsActive));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateWorkerRequest request)
    {
        var worker = await db.Users.SingleOrDefaultAsync(x => x.Id == id && x.Role == "Worker");
        if (worker is null) return NotFound();
        worker.FullName = request.FullName.Trim(); worker.IsActive = request.IsActive;
        if (!string.IsNullOrWhiteSpace(request.NewPassword)) worker.PasswordHash = new PasswordHasher<User>().HashPassword(worker, request.NewPassword);
        await db.SaveChangesAsync(); return NoContent();
    }
}
