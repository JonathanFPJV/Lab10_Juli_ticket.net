using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab10_Juli.Infrastructure.Adapters.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    private readonly TicketerabdContext _context;

    public TicketRepository(TicketerabdContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetByIdOptimizedAsync(Guid id)
    {
        return await _context.Set<Ticket>()
            .Include(t => t.User) 
            .Include(t => t.Responses) 
            .ThenInclude(r => r.Responder) 
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TicketId == id);
    }

    public async Task<IEnumerable<Ticket>> GetAllOptimizedAsync()
    {
        return await _context.Set<Ticket>()
            .Include(t => t.User) 
            .AsNoTracking()
            .ToListAsync();
    }
}