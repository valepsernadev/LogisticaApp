namespace LogisticaApp.Models;

public class Incidencia
{
    public Guid Id { get; set; }
    public Guid EnvioId { get; set; }
    public Guid ConductorId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Envio Envio { get; set; } = null!;
    public Conductor Conductor { get; set; } = null!;
}