namespace Lab10_Juli.Application.DTOs;

public class TicketDto
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }

    // Inicializamos para evitar advertencias de nulidad
    public string Title { get; set; } = string.Empty; 
        
    // Este sí puede ser nulo
    public string? Description { get; set; } 

    // Inicializamos
    public string Status { get; set; } = string.Empty; 
        
    public DateTime? CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    // Dato "aplanado" desde la entidad User
    public string UserName { get; set; } = string.Empty;

    // Usamos el DTO de Response, no la entidad
    public virtual ICollection<ResponseDto> Responses { get; set; } = new List<ResponseDto>();
}

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
}