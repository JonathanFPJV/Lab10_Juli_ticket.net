using System.Security.Claims;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketUseCase _ticketUseCase;

    public TicketsController(ITicketUseCase ticketUseCase)
    {
        _ticketUseCase = ticketUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _ticketUseCase.GetAllAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _ticketUseCase.GetByIdAsync(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketDto createDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Usuario no autenticado o ID inválido.");
        }

        var newTicket = await _ticketUseCase.CreateAsync(createDto, userId);
        return CreatedAtAction(nameof(GetById), new { id = newTicket.TicketId }, newTicket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTicketDto updateDto)
    {
        var success = await _ticketUseCase.UpdateAsync(id, updateDto);
        if (!success) return NotFound();
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _ticketUseCase.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}