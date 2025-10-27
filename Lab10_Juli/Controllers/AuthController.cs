using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;

    public AuthController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerDto)
    {
        try
        {
            var userDto = await _authUseCase.RegisterAsync(registerDto);
            // Devuelve 201 Created
            return CreatedAtAction(nameof(Register), new { id = userDto.UserId }, userDto);
        }
        catch (ApplicationException ex)
        {
            // Devuelve 409 Conflict si el usuario ya existe
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginDto)
    {
        var loginResponse = await _authUseCase.LoginAsync(loginDto);
            
        if (loginResponse == null)
        {
            // Devuelve 401 Unauthorized si las credenciales son incorrectas
            return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });
        }

        // Devuelve 200 OK con el token
        return Ok(loginResponse);
    }
}