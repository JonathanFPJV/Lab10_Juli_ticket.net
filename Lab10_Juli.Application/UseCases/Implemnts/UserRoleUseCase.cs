using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Services;

public class UserRoleUseCase : IUserRoleUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllAsync()
        {
            // Usamos el repo optimizado que trae User y Role
            var userRoles = await _unitOfWork.UserRoleRepository.GetAllOptimizedAsync();
            return _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);
        }

        public async Task<UserRoleDto?> AssignRoleAsync(CreateUserRoleDto createDto)
        {
            // 1. Verificar que no exista ya
            var existing = await _unitOfWork.UserRoleRepository.GetByIdsAsync(createDto.UserId, createDto.RoleId);
            if (existing != null)
            {
                // Ya existe, no hacemos nada o lanzamos excepción
                return null; 
            }

            // 2. Crear la entidad
            var userRole = _mapper.Map<UserRole>(createDto);
            userRole.AssignedAt = DateTime.UtcNow;

            // 3. Usar el 'Add' genérico (¡aquí sí usamos el genérico!)
            await _unitOfWork.UserRoleRepository.Add(userRole);
            await _unitOfWork.Complete();

            // 4. Devolver el DTO completo
            var newUserRole = await _unitOfWork.UserRoleRepository.GetByIdsAsync(createDto.UserId, createDto.RoleId);
            return _mapper.Map<UserRoleDto>(newUserRole);
        }

        public async Task<bool> RemoveRoleAsync(CreateUserRoleDto removeDto)
        {
            // Usamos el método optimizado para borrar por llave compuesta
            bool success = await _unitOfWork.UserRoleRepository.DeleteByIdsAsync(removeDto.UserId, removeDto.RoleId);
            
            if (success)
            {
                await _unitOfWork.Complete(); // Guardamos los cambios
            }
            
            return success;
        }
    }