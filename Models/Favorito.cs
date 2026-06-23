namespace LogisticaApp.Models;

public class Favorito
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Usuario Cliente { get; set; } = null!;
}