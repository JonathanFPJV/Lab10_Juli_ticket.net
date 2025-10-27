using Lab10_Juli.Application.DTOs;

namespace Lab10_Juli.Domain.Ports.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(Guid id);
    Task<RoleDto> CreateAsync(RoleCreateDto createDto);
    Task UpdateAsync(Guid id, RoleUpdateDto updateDto);
    Task DeleteAsync(Guid id);
}