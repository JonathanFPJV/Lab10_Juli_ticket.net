namespace Lab10_Juli.Domain.Ports.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    IResponseRepository ResponseRepository { get; }
    ITicketRepository TicketRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IUserRepository Users { get; }
    Task<int> Complete();
}