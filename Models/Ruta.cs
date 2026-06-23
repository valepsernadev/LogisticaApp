namespace LogisticaApp.Models;

public class Ruta
{
    public Guid Id { get; set; }
    public Guid EnvioId { get; set; }
    public decimal? DistanciaKm { get; set; }
    public int? TiempoEstimadoMinutos { get; set; }
    public DateTime CreatedAt { get; set; }

    public Envio Envio { get; set; } = null!;
}