namespace LogisticaApp.Models;

public class Evidencia
{
    public Guid Id { get; set; }
    public Guid EnvioId { get; set; }
    public Guid ConductorId { get; set; }
    public string FotoUrl { get; set; } = string.Empty;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime CreatedAt { get; set; }

    public Envio Envio { get; set; } = null!;
    public Conductor Conductor { get; set; } = null!;
}