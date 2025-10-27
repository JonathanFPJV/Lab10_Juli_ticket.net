using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleUseCase _userRoleUseCase;

    public UserRolesController(IUserRoleUseCase userRoleUseCase)
    {
        _userRoleUseCase = userRoleUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userRoles = await _userRoleUseCase.GetAllAsync();
        return Ok(userRoles);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(CreateUserRoleDto createDto)
    {
        var result = await _userRoleUseCase.AssignRoleAsync(createDto);
        if (result == null)
        {
            return Conflict(new { Message = "La asignación ya existe." });
        }
        return Ok(result);
    }

    // Usamos [FromBody] en DELETE, lo cual es común para llaves compuestas
    [HttpDelete]
    public async Task<IActionResult> RemoveRole([FromBody] CreateUserRoleDto removeDto)
    {
        var success = await _userRoleUseCase.RemoveRoleAsync(removeDto);
        if (!success)
        {
            return NotFound(new { Message = "La asignación no fue encontrada." });
        }
        return NoContent(); // 204 No Content
    }
}