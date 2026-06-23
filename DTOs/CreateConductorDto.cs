namespace LogisticaApp.DTOs;

public class CreateConductorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Licencia { get; set; } = string.Empty;
}