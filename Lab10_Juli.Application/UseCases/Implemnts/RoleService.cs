using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _unitOfWork.Repository<Role>().GetAll();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id)
    {
        var role = await _unitOfWork.Repository<Role>().GetById(id);
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> CreateAsync(RoleCreateDto createDto)
    {
        // 1. Mapea el DTO a la Entidad
        var role = _mapper.Map<Role>(createDto);

        // 2. Agrega al repositorio (en memoria)
        await _unitOfWork.Repository<Role>().Add(role);
        
        // 3. Guarda los cambios en la BD a través del UoW
        await _unitOfWork.Complete();

        // 4. Mapea la entidad (ya con su ID) de vuelta a un DTO y la retorna
        return _mapper.Map<RoleDto>(role);
    }

    public async Task UpdateAsync(Guid id, RoleUpdateDto updateDto)
    {
        var existingRole = await _unitOfWork.Repository<Role>().GetById(id);
        if (existingRole == null)
        {
            throw new KeyNotFoundException("Rol no encontrado");
        }

        // AutoMapper actualiza el objeto 'existingRole' con los datos del DTO
        _mapper.Map(updateDto, existingRole);

        await _unitOfWork.Repository<Role>().Update(existingRole);
        await _unitOfWork.Complete();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingRole = await _unitOfWork.Repository<Role>().GetById(id);
        if (existingRole == null)
        {
            throw new KeyNotFoundException("Rol no encontrado");
        }

        await _unitOfWork.Repository<Role>().Delete(id);
        await _unitOfWork.Complete();
    }
}