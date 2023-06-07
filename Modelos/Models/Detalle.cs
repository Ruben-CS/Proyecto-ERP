using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class Detalle
{
    [Key]
    public int IdDetalleVenta { get; set; }

    public int       IdArticulo { get; set; }
    public Articulo? Articulo   { get; set; }

    public int NroLote { get; set; }

    [ForeignKey("Nota")]
    public int IdNota { get; set; }
    public Nota?    Nota        { get; set; }
    public int     Cantidad    { get; set; }
    public decimal PrecioVenta { get; set; }
}