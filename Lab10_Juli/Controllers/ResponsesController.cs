using System.Security.Claims;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponsesController : ControllerBase
{
    // Usamos IResponseUseCase (o IResponseService)
    private readonly IResponseUseCase _responseUseCase;

    public ResponsesController(IResponseUseCase responseUseCase)
    {
        _responseUseCase = responseUseCase;
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetByTicket(Guid ticketId)
    {
        // Llama al método del UseCase (que ahora es súper eficiente)
        var responses = await _responseUseCase.GetResponsesForTicketAsync(ticketId);
        return Ok(responses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _responseUseCase.GetByIdAsync(id);
        if (response == null) return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateResponseDto createDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Usuario no autenticado.");
        }
        var responderId = Guid.Parse(userIdString);

        var newResponse = await _responseUseCase.CreateAsync(createDto, responderId);
        
        return CreatedAtAction(nameof(GetById), new { id = newResponse.ResponseId }, newResponse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateResponseDto updateDto)
    {
        var success = await _responseUseCase.UpdateAsync(id, updateDto);
        if (!success) return NotFound(new { Message = "Respuesta no encontrada" });
        
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _responseUseCase.DeleteAsync(id);
        if (!success) return NotFound(new { Message = "Respuesta no encontrada" });

        return NoContent(); 
    }
}