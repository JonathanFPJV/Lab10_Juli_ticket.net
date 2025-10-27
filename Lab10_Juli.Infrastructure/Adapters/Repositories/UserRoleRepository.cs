using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab10_Juli.Infrastructure.Adapters.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly TicketerabdContext _context;
    public UserRoleRepository(TicketerabdContext context) : base(context)
    {
        _context = context; // Guardamos el context localmente
    }

    public async Task<IEnumerable<UserRole>> GetAllOptimizedAsync()
    {
        // Hacemos JOIN con User y con Role para obtener los nombres
        return await _context.Set<UserRole>()
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserRole?> GetByIdsAsync(Guid userId, Guid roleId)
    {
        return await _context.Set<UserRole>()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public async Task<bool> DeleteByIdsAsync(Guid userId, Guid roleId)
    {
        var userRole = await GetByIdsAsync(userId, roleId);
        if (userRole == null) return false;

        // Usamos el 'Remove' del DbContext (no el 'Delete(Guid id)' genérico)
        _context.Set<UserRole>().Remove(userRole);
        return true;
    }
}