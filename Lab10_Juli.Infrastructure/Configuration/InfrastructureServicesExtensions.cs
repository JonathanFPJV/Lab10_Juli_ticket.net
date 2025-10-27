using System.Reflection;
using Lab10_Juli.Domain.Ports.Repositories;
using Lab10_Juli.Domain.Ports.Services;
using Lab10_Juli.Infrastructure.Adapters.Repositories;
using Lab10_Juli.Infrastructure.Adapters.Security;
using Lab10_Juli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab10_Juli.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Conexión a la Base de Datos
        services.AddDbContext<TicketerabdContext>((IServiceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection"); 
            options.UseNpgsql(connectionString);
        });
        
        
        // 2. Registros de Repositorios y UnitOfWork (Adaptadores)
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IResponseRepository, ResponseRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();


        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
    
    
}