using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;

namespace Modelos.Models;

public class Comprobante
{
    [Key]
    public int IdComprobante { get; set; }

    public string Serie { get; set; } = null!;
    public string Glosa { get; set; } = null!;

    [Required]
    public DateTime Fecha { get; set; }

    public string            Tc              { get; set; } = null!;
    public EstadoComprobante Estado          { get; set; } = EstadoComprobante.Abierto;
    public string            TipoComprobante { get; set; } = null!;

    public List<DetalleComprobante>? DetalleComprobantes { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [ForeignKey("Moneda")]
    public int IdMoneda { get; set; }

    public Moneda? Moneda { get; set; }

    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [InverseProperty("Comprobantes")]
    public Empresa? Empresa { get; set; }
}