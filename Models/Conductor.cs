namespace LogisticaApp.Models;

public class Conductor
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Licencia { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }

    public Usuario Usuario { get; set; } = null!;
}