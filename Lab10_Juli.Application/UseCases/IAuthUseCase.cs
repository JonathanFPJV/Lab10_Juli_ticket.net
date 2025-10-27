using Lab10_Juli.Application.DTOs;

namespace Lab10_Juli.Domain.Ports.Services;

public interface IAuthUseCase
{
    // Devuelve el DTO del usuario (sin hash)
    Task<UserDto> RegisterAsync(RegisterUserDto registerDto);

    // Devuelve el DTO con el Token JWT
    Task<LoginResponseDto?> LoginAsync(LoginUserDto loginDto);
}