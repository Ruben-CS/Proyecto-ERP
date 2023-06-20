namespace Modelos.Models;

public class ResultadoLibroDiario
{
    public List<ComprobanteConDetalles> Comprobantes { get; set; }
    public decimal                      TotalDebe    { get; set; }
    public decimal                      TotalHaber   { get; set; }
}