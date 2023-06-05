using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class Detalle
{
    [Key]
    [Column(Order = 1)]
    [ForeignKey("Articulo")]
    public int IdArticulo { get;    set; }
    public Articulo Articulo { get; set; }

    [Key]
    [Column(Order = 2)]
    [ForeignKey("Lote")]
    public int NroLote { get; set; }
    public Lote Lote { get;   set; }

    [Key]
    [Column(Order = 3)]
    [ForeignKey("Nota")]
    public int IdNota { get; set; }
    public Nota Nota { get;  set; }

    public int     Cantidad    { get; set; }
    public decimal PrecioVenta { get; set; }
}