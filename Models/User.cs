namespace BENom.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } // Contraseña encriptada
    public string Role { get; set; } // Ejemplo: "Admin", "User"
}
