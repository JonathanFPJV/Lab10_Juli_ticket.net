using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Repositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetAllOptimizedAsync();
    Task<UserRole?> GetByIdsAsync(Guid userId, Guid roleId);
    Task<bool> DeleteByIdsAsync(Guid userId, Guid roleId);
}