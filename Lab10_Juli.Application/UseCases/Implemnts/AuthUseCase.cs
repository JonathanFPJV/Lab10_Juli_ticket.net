using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Services;

public class AuthUseCase : IAuthUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<UserDto> RegisterAsync(RegisterUserDto registerDto)
    {
        // 1. Verificar si el usuario ya existe
        var existingUser = await _unitOfWork.Users.GetByUsernameOptimizedAsync(registerDto.Username);
        if (existingUser != null)
        {
            throw new ApplicationException("El nombre de usuario ya existe.");
        }

        // 2. Mapear DTO a Entidad (ignorando Password)
        var user = _mapper.Map<User>(registerDto);

        // 3. Hashear la contraseña (¡Aquí!)
        user.PasswordHash = _passwordHasher.Hash(registerDto.Password);
        
        user.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        // 4. Guardar en la BD
        await _unitOfWork.Users.Add(user);
        await _unitOfWork.Complete();

        // 5. Devolver el DTO seguro (sin hash)
        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginUserDto loginDto)
    {
        // 1. Buscar al usuario (CON SUS ROLES)
        var user = await _unitOfWork.Users.GetByUsernameOptimizedAsync(loginDto.Username);
        if (user == null)
        {
            return null; // Usuario no encontrado
        }

        // 2. Verificar la contraseña
        bool isPasswordValid = _passwordHasher.Verify(user.PasswordHash, loginDto.Password);
        if (!isPasswordValid)
        {
            return null; // Contraseña incorrecta
        }

        // 3. Obtener los nombres de los roles
        var roles = user.UserRoles.Select(ur => ur.Role.RoleName);

        // 4. Generar el Token JWT
        string token = _jwtTokenGenerator.GenerateToken(user, roles);

        // 5. Devolver la respuesta
        return new LoginResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user)
        };
    }
}