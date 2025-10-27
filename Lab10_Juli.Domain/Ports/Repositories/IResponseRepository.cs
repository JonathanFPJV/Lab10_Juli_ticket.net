using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Repositories;

public interface IResponseRepository: IGenericRepository<Response>
{
    Task<IEnumerable<Response>> GetResponsesForTicketOptimizedAsync(Guid ticketId);
}