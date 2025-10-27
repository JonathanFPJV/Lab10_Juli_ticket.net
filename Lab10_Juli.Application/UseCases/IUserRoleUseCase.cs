using Lab10_Juli.Application.DTOs;

namespace Lab10_Juli.Domain.Ports.Services;

public interface IUserRoleUseCase
{
    Task<IEnumerable<UserRoleDto>> GetAllAsync();
    Task<UserRoleDto?> AssignRoleAsync(CreateUserRoleDto createDto);
    Task<bool> RemoveRoleAsync(CreateUserRoleDto removeDto);
}