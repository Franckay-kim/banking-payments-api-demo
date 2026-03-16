using BankingPaymentsApiDemo.DTOs;
using BankingPaymentsApiDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingPaymentsApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;

    public AuthController(IConfiguration configuration, IJwtService jwtService)
    {
        _configuration = configuration;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        var configuredUsername = _configuration["AdminUser:Username"];
        var configuredPassword = _configuration["AdminUser:Password"];

        if (request.Username != configuredUsername || request.Password != configuredPassword)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var token = _jwtService.GenerateToken(request.Username);

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddHours(2)
        });
    }
}