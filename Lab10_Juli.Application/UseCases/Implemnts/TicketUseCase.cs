using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Services;

public class TicketUseCase : ITicketUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllOptimizedAsync();
            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<TicketDto?> GetByIdAsync(Guid id)
        {
            // Usamos el repo optimizado que trae 'User' Y 'Responses'
            var ticket = await _unitOfWork.TicketRepository.GetByIdOptimizedAsync(id);
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> CreateAsync(CreateTicketDto createDto, Guid userId)
        {
            var ticket = _mapper.Map<Ticket>(createDto);
            
            // Asignamos datos del servidor
            ticket.UserId = userId;
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = "Abierto"; // Estado inicial por defecto

            await _unitOfWork.TicketRepository.Add(ticket);
            await _unitOfWork.Complete();

            // Devolvemos el ticket completo
            return (await GetByIdAsync(ticket.TicketId))!;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateTicketDto updateDto)
        {
            // Para actualizar, no necesitamos los datos optimizados
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);
            if (existingTicket == null) return false;

            // Mapeamos los campos (Title, Description, Status)
            _mapper.Map(updateDto, existingTicket);

            // Si el estado es "Cerrado", ponemos fecha de cierre
            if (updateDto.Status.Equals("Cerrado", StringComparison.OrdinalIgnoreCase) 
                && existingTicket.ClosedAt == null)
            {
                existingTicket.ClosedAt = DateTime.UtcNow;
            }

            await _unitOfWork.TicketRepository.Update(existingTicket);
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);
            if (existingTicket == null) return false;

            await _unitOfWork.TicketRepository.Delete(id);
            await _unitOfWork.Complete();
            return true;
        }
    }