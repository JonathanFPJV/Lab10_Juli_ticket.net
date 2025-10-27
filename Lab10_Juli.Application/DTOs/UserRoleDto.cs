namespace Lab10_Juli.Application.DTOs;

public class UserRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? AssignedAt { get; set; }

    // Dato aplanado de la entidad 'User'
    public string UserName { get; set; } = string.Empty;

    // Dato aplanado de la entidad 'Role'
    public string RoleName { get; set; } = string.Empty;
}

public class CreateUserRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}