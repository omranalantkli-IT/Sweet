using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetFactory.Application.Contracts;
using SweetFactory.Application.Services;

namespace SweetFactory.Api.Controllers;

[ApiController, Route("api/auth")]
public class AuthController(AuthService auth) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await auth.LoginAsync(request);
        return response is null
            ? Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة" })
            : Ok(response);
    }

    [Authorize, HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await auth.GetUserAsync(id);
        return user is null ? NotFound() : Ok(user);
    }
}
