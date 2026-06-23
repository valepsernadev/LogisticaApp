namespace LogisticaApp.Models;

public class Envio
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public Guid? ConductorId { get; set; }
    public Guid? VehiculoId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string Destino { get; set; } = string.Empty;
    public string? TipoMercancia { get; set; }
    public decimal? PesoKg { get; set; }
    public string? Observaciones { get; set; }
    public DateTime? FechaEstimadaEntrega { get; set; }
    public DateTime CreatedAt { get; set; }

    public Usuario Cliente { get; set; } = null!;
    public Conductor? Conductor { get; set; }
    public Vehiculo? Vehiculo { get; set; }
}