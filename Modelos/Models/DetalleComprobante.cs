using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class DetalleComprobante
{
    [Key]
    public int IdDetalleComprobante { get; set; }

    public int     Numero        { get; set; }
    public string  Glosa         { get; set; }
    public decimal MontoDebe     { get; set; }
    public decimal MontoHaber    { get; set; }
    public decimal MontoDebeAlt  { get; set; }
    public decimal MontoHaberAlt { get; set; }
    public string? NombreCuenta  { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; } = 1;

    public Usuario? Usuario { get; set; }

    [ForeignKey("Comprobante")]
    public int IdComprobante { get; set; }

    [InverseProperty("DetalleComprobantes")]
    public Comprobante? Comprobante { get; set; }


    [ForeignKey("Cuenta")]
    public int IdCuenta { get; set; }

    [InverseProperty("DetalleComprobantes")]
    public Cuenta? Cuenta { get; set; }
}