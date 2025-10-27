namespace Lab10_Juli.Application.DTOs;

public class RoleDto
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}

public class RoleCreateDto
{
    public string RoleName { get; set; }
}

public class RoleUpdateDto
{
    public string RoleName { get; set; }
}