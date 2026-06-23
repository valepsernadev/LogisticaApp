namespace LogisticaApp.DTOs;

public class UpdateConductorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Licencia { get; set; } = string.Empty;
    public bool Activo { get; set; }
}