namespace Modelos.Models;

public class DetallesComprobantesReporte
{
    public string? NombreCuenta { get; set; }
    public string? Glosa        { get; set; }
    public decimal?  MontoDebe    { get; set; }
    public decimal?  MontoHaber   { get; set; }
}