namespace Lab10_Juli.Application.DTOs;

public class RegisterUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // ¡Texto plano!
}

// --- DTO 2: Para Iniciar Sesión ---
public class LoginUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // ¡Texto plano!
}

// --- DTO 3: La Respuesta de Login (El Token JWT) ---
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } // Opcional: devolver datos del usuario
}

// --- DTO 4: Para Mostrar un Usuario (¡SIN HASH!) ---
public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
        
    // Aplanamos los roles para que sea más útil
    public ICollection<string> Roles { get; set; } = new List<string>();
}