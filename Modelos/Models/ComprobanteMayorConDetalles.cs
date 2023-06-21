namespace Modelos.Models;

public class ComprobanteMayorConDetalles
{
    public int?                    IdComprobante { get; set; }
    public List<LibroMayorReporte> Detalles      { get; set; }

}