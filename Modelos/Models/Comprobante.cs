using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class Comprobante
{
    [Key]
    public int IdComprobante { get; set; }

    public string Serie { get; set; }
    public string Glosa { get; set; }
    [Required]
    public DateTime Fecha { get;         set; }
    public string TC              { get; set; }
    public bool   Estado          { get; set; }
    public string TipoComprobante { get; set; }

    public List<DetalleComprobante>? DetalleComprobantes { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [ForeignKey("Moneda")]
    public int IdMoneda { get;   set; }
    public Moneda? Moneda { get; set; }

}