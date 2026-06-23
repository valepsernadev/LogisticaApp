namespace LogisticaApp.Models;

public class Vehiculo
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public decimal CapacidadKg { get; set; }
    public Guid? ConductorId { get; set; }
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }

    public Conductor? Conductor { get; set; }
}