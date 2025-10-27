using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Repositories;

public interface ITicketRepository: IGenericRepository<Ticket>
{
    Task<Ticket?> GetByIdOptimizedAsync(Guid id);
    Task<IEnumerable<Ticket>> GetAllOptimizedAsync();
}