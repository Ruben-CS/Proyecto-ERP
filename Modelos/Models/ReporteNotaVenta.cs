using Modelos.Models.Enums;

namespace Modelos.Models;

public class ReporteNotaVenta
{
    public int        NroLoteOficial { get; set; }
    public EstadoNota Estado         { get; set; }
    public string     NombreEmpresa  { get; set; }
    public int        IdNota         { get; set; }
    public int        NroNota        { get; set; }
    public string?    Descripcion    { get; set; }
    public DateTime   Fecha          { get; set; }

    public decimal  Total          { get; set; }
    public TipoNota Tipo           { get; set; }
    public string   NombreArticulo { get; set; }
    public int      Cantidad       { get; set; }
    public decimal  Precio         { get; set; }
    public decimal  Subtotal       { get; set; }
}