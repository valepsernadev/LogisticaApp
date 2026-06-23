namespace LogisticaApp.Models;

public class Notificacion
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public bool Leida { get; set; }
    public DateTime CreatedAt { get; set; }

    public Usuario Usuario { get; set; } = null!;
}