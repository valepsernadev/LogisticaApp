using Microsoft.AspNetCore.Mvc;
using LogisticaApp.DTOs;
using LogisticaApp.Interfaces;

namespace LogisticaApp.Controllers;

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
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var data = await _authService.Register(dto);
        return Created(string.Empty, new ApiResponseDto<AuthResponseDto>
        {
            Success = true,
            Message = "Usuario registrado exitosamente",
            Data = data,
            StatusCode = 201
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var data = await _authService.Login(dto);
        return Ok(new ApiResponseDto<AuthResponseDto>
        {
            Success = true,
            Message = "Login exitoso",
            Data = data,
            StatusCode = 200
        });
    }
}