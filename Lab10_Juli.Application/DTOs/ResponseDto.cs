namespace Lab10_Juli.Application.DTOs;

public class ResponseDto
{
    public Guid ResponseId { get; set; }
    public Guid TicketId { get; set; }
    public Guid ResponderId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }

    // Dato aplanado de la entidad 'Responder'
    public string ResponderName { get; set; } = string.Empty;
}

public class CreateResponseDto
{
    public Guid TicketId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class UpdateResponseDto
{
    public string Message { get; set; } = string.Empty;
}