using CvForgeAI.Application.DTO.Auth;
using CvForgeAI.Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CvForgeAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var fullName = User.FindFirst(ClaimTypes.Name)?.Value;

        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            UserId = userId,
            FullName = fullName,
            Email = email,
            Role = role
        });
    }
}