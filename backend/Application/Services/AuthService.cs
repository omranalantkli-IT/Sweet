using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Application.Contracts;
using SweetFactory.Domain.Entities;
using SweetFactory.Infrastructure.Authentication;
using SweetFactory.Infrastructure.Persistence;

namespace SweetFactory.Application.Services;

public class AuthService(AppDbContext db, TokenService tokens)
{
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(candidate => candidate.Username == username);
        if (user is null || !user.IsActive || new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return null;
        return new AuthResponse(tokens.Create(user), ToDto(user));
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        var user = await db.Users.FindAsync(id);
        return user is null ? null : ToDto(user);
    }

    private static UserDto ToDto(User user) =>
        new(user.Id, user.FullName, user.Username, user.Role, user.IsActive);
}
