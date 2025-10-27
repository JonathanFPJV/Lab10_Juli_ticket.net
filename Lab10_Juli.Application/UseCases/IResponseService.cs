using Lab10_Juli.Application.DTOs;

namespace Lab10_Juli.Domain.Ports.Services;

public interface IResponseUseCase
{
    Task<ResponseDto?> GetByIdAsync(Guid id);

    Task<IEnumerable<ResponseDto>> GetResponsesForTicketAsync(Guid ticketId);

    Task<ResponseDto> CreateAsync(CreateResponseDto createDto, Guid responderId);

    Task<bool> UpdateAsync(Guid id, UpdateResponseDto updateDto);

    Task<bool> DeleteAsync(Guid id);
}