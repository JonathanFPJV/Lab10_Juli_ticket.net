using System.Reflection;
using Lab10_Juli.Domain.Ports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab10_Juli.Application.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registra AutoMapper, buscando perfiles en todo el ensamblado de 'Application'
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registra tus Casos de Uso (lo que antes llamabas servicios)
        
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IResponseUseCase, ResponseUseCase>();
        services.AddScoped<ITicketUseCase, TicketUseCase>();
        services.AddScoped<IUserRoleUseCase, UserRoleUseCase>();
        services.AddScoped<IAuthUseCase, AuthUseCase>();
        // Aquí registrarías otros casos de uso
        // services.AddScoped<IProductoUseCase, ProductoUseCase>();
        // services.AddScoped<IAuthUseCase, AuthUseCase>();

        return services;
    }
}