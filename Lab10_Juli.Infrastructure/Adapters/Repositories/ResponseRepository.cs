using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab10_Juli.Infrastructure.Adapters.Repositories;

public class ResponseRepository : GenericRepository<Response>, IResponseRepository
{ 
    private readonly TicketerabdContext context; 
    public ResponseRepository(TicketerabdContext context) : base(context)
    {
        this.context = context; 
    }
    
    public async Task<IEnumerable<Response>> GetResponsesForTicketOptimizedAsync(Guid ticketId)
    {
        return await context.Set<Response>()
            .Where(r => r.TicketId == ticketId)
            .Include(r => r.Responder)
            .AsNoTracking()
            .ToListAsync(); 
    }
}