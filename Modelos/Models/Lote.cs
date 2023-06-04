using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models;

public class Lote
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdLote { get; set; }

    [ForeignKey("Articulo")]
    public int IdArticulo { get;    set; }
    public Articulo Articulo { get; set; }

    public int? NroLote { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public int Cantidad { get; set; }

    public EstadoLote EstadoLote { get; set; }

    public decimal PrecioCompra { get; set; }

    public int Stock { get; set; }

    [ForeignKey("Nota")]
    public int IdNota { get; set; }
    public Nota Nota { get;  set; }
}