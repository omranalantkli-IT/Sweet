using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetFactory.Data;
using SweetFactory.Dtos;
using SweetFactory.Models;
using SweetFactory.Services;

namespace SweetFactory.Controllers;

[ApiController, Route("api/auth")]
public class AuthController(AppDbContext db, TokenService tokens) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Username == username);
        if (user is null || !user.IsActive || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" });
        var dto = new UserDto(user.Id, user.FullName, user.Username, user.Role, user.IsActive);
        return Ok(new AuthResponse(tokens.Create(user), dto));
    }

    [Authorize, HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(new UserDto(user.Id, user.FullName, user.Username, user.Role, user.IsActive));
    }
}
