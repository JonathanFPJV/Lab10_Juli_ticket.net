using Lab10_Juli.Application.DTOs;

namespace Lab10_Juli.Domain.Ports.Services;

public interface ITicketUseCase
{
    Task<IEnumerable<TicketDto>> GetAllAsync();
    Task<TicketDto?> GetByIdAsync(Guid id);
    Task<TicketDto> CreateAsync(CreateTicketDto createDto, Guid userId);
    Task<bool> UpdateAsync(Guid id, UpdateTicketDto updateDto);
    Task<bool> DeleteAsync(Guid id);
}