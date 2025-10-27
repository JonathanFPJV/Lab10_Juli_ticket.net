using System.Collections;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Infrastructure.Adapters.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketerabdContext _context;
    private Hashtable? _repositories;

    private IResponseRepository? _responseRepository;
    private ITicketRepository? _ticketRepository;
    private IUserRoleRepository? _userRoleRepository;
    private IUserRepository? _userRepository;
    
    public UnitOfWork(TicketerabdContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    // Método genérico existente (NO SE TOCA)
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;

        if (_repositories!.ContainsKey(type))
        {
            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        var repositoryType = typeof(GenericRepository<>);
        var repositoryInstance = Activator.CreateInstance(
            repositoryType.MakeGenericType(typeof(TEntity)), _context);

        if (repositoryInstance != null)
        {
            _repositories.Add(type, repositoryInstance);
            return (IGenericRepository<TEntity>)repositoryInstance;
        }

        throw new Exception($"No se pudo crear el repositorio para el tipo {type}");
    }

    public IResponseRepository ResponseRepository => _responseRepository ??= new ResponseRepository(_context);
    public ITicketRepository TicketRepository => _ticketRepository ??= new TicketRepository(_context);
    public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(_context);
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    // Guardar cambios
    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}