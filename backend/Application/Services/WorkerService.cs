using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Application.Contracts;
using SweetFactory.Domain.Entities;
using SweetFactory.Infrastructure.Persistence;

namespace SweetFactory.Application.Services;

public class WorkerService(AppDbContext db)
{
    public async Task<IReadOnlyList<UserDto>> GetAsync() => await db.Users.AsNoTracking()
        .Where(worker => worker.Role == "Worker").OrderBy(worker => worker.FullName)
        .Select(worker => new UserDto(worker.Id, worker.FullName, worker.Username, worker.Role, worker.IsActive))
        .ToListAsync();

    public async Task<UserDto?> CreateAsync(CreateWorkerRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(user => user.Username == username)) return null;
        var worker = new User
        {
            FullName = request.FullName.Trim(),
            Username = username,
            PasswordHash = string.Empty,
            Role = "Worker"
        };
        worker.PasswordHash = new PasswordHasher<User>().HashPassword(worker, request.Password);
        db.Users.Add(worker);
        await db.SaveChangesAsync();
        return new UserDto(worker.Id, worker.FullName, worker.Username, worker.Role, worker.IsActive);
    }

    public async Task<bool> UpdateAsync(int id, UpdateWorkerRequest request)
    {
        var worker = await db.Users.SingleOrDefaultAsync(user => user.Id == id && user.Role == "Worker");
        if (worker is null) return false;
        worker.FullName = request.FullName.Trim();
        worker.IsActive = request.IsActive;
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
            worker.PasswordHash = new PasswordHasher<User>().HashPassword(worker, request.NewPassword);
        await db.SaveChangesAsync();
        return true;
    }
}
