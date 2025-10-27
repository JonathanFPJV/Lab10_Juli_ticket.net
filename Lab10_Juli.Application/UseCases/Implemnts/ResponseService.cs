using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Services;

public class ResponseUseCase: IResponseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ResponseUseCase( IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto?> GetByIdAsync(Guid id)
    {
        var response = await _unitOfWork.Repository<Response>().GetById(id);
        if (response == null) return null;

        // Cargamos el 'User' (Responder) para obtener el nombre
        var responder = await _unitOfWork.Repository<User>().GetById(response.ResponderId);
        
        var responseDto = _mapper.Map<ResponseDto>(response);
        
        // Corregido: 'UserName' (PascalCase) es la convención estándar en C#
        responseDto.ResponderName = responder?.Username ?? "Usuario Desconocido"; 

        return responseDto;
    }

    public async Task<IEnumerable<ResponseDto>> GetResponsesForTicketAsync(Guid ticketId)
    {
        // ¡¡OPTIMIZADO!!
        // Ya no usamos 'FindAll' ni filtramos en memoria.
        // Usamos el método optimizado del repositorio específico.
        var responses = await _unitOfWork.ResponseRepository.GetResponsesForTicketOptimizedAsync(ticketId);
            
        // AutoMapper se encarga del resto, ya que 'responses'
        // ahora incluye la entidad 'Responder'.
        return _mapper.Map<IEnumerable<ResponseDto>>(responses);
    }

    public async Task<ResponseDto> CreateAsync(CreateResponseDto createDto, Guid responderId)
    {
        // Mapeamos DTO a Entidad
        var response = _mapper.Map<Response>(createDto);
        
        // Asignamos los datos del servidor
        response.ResponderId = responderId;
        response.CreatedAt = DateTime.UtcNow;

        // Guardamos en la BD usando el patrón de tu UoW
        await _unitOfWork.Repository<Response>().Add(response);
        await _unitOfWork.Complete(); 

        // Devolvemos el DTO completo llamando a nuestro propio método GetById
        var newDto = await GetByIdAsync(response.ResponseId);
        return newDto!; 
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateResponseDto updateDto)
    {
        var existingResponse = await _unitOfWork.Repository<Response>().GetById(id);
        if (existingResponse == null)
        {
            return false; // No se encontró
        }

        // AutoMapper actualiza la entidad 'existingResponse' solo con los campos de 'updateDto'
        _mapper.Map(updateDto, existingResponse);

        await _unitOfWork.Repository<Response>().Update(existingResponse);
        await _unitOfWork.Complete();
        return true; // Éxito
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingResponse = await _unitOfWork.Repository<Response>().GetById(id);
        if (existingResponse == null)
        {
            return false; // No se encontró
        }

        await _unitOfWork.Repository<Response>().Delete(id);
        await _unitOfWork.Complete();
        return true; // Éxito
    }
    
}