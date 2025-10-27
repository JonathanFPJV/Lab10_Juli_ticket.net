using AutoMapper;
using Lab10_Juli.Application.DTOs;
using Lab10_Juli.Infrastructure.Data;

namespace Lab10_Juli.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entidad Rol
        CreateMap<Role, RoleDto>();
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>();
        //Entidad Response
        CreateMap<Response, ResponseDto>()
            .ForMember(
                dest => dest.ResponderName, 
                opt => opt.MapFrom(src => src.Responder.Username)
            );
        CreateMap<CreateResponseDto, Response>();
        CreateMap<UpdateResponseDto, Response>();
        //Entidad ticket
        CreateMap<Ticket, TicketDto>()
            .ForMember(
                dest => dest.UserName, // El destino en TicketDto
                opt => opt.MapFrom(src => src.User.Username)
            );
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<UpdateTicketDto, Ticket>();
        //Entidad UserRole
        CreateMap<UserRole, UserRoleDto>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.Username) // Asume que User tiene UserName
            )
            .ForMember(
                dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role.RoleName) // Asume que Role tiene RoleName
            );
        CreateMap<CreateUserRoleDto, UserRole>();
        //Entidad User
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName))
            );
        
        // 2. De RegisterUserDto -> Entidad User (Para crear)
        CreateMap<RegisterUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


    }
}