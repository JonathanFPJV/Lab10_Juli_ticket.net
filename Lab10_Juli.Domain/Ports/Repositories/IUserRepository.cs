using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Domain.Ports.Repositories;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetByUsernameOptimizedAsync(string username);
}