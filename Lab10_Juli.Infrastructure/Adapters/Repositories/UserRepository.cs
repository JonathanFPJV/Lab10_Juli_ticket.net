using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab10_Juli.Infrastructure.Adapters.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly TicketerabdContext _context;
    public UserRepository(TicketerabdContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameOptimizedAsync(string username)
    {
        return await _context.Set<User>()
            .Include(u => u.UserRoles)       
            .ThenInclude(ur => ur.Role) // Incluye la entidad Role
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}