namespace LogisticaApp.Models;

public class Tarifa
{
    public Guid Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreatedAt { get; set; }
}