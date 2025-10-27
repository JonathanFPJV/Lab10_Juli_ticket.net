using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    // GET: api/roles/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await _roleService.GetByIdAsync(id);
        
        if (role == null)
        {
            return NotFound("Rol no encontrado.");
        }
        
        return Ok(role);
    }

    // POST: api/roles
    [HttpPost]
    public async Task<IActionResult> Create(RoleCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var newRole = await _roleService.CreateAsync(createDto);
        
        // Devuelve un 201 Created con la ubicación del nuevo recurso y el recurso creado
        return CreatedAtAction(nameof(GetById), new { id = newRole.RoleId }, newRole);
    }

    // PUT: api/roles/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RoleUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _roleService.UpdateAsync(id, updateDto);
            return NoContent(); // 204 No Content (Éxito, sin nada que devolver)
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/roles/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _roleService.DeleteAsync(id);
            return NoContent(); // 204 No Content
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}