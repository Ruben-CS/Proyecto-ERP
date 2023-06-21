namespace Modelos.Models;

public class ComprobanteConDetalles
{
    public int                      IdComprobante { get; set; }
    public List<LibroDiarioReporte> Detalles      { get; set; }

}