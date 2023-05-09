using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class DetalleComprobante
{
    [Key]
    public int IdDetalleComprobante { get; set; }

    public int    Numero        { get; set; }
    public string Glosa         { get; set; }
    public float  MontoDebe     { get; set; }
    public float  MontoHaber    { get; set; }
    public float  MontoDebeAlt  { get; set; }
    public float  MontoHaberAlt { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [ForeignKey("Comprobante")]
    public int IdComprobante { get; set; }

    [InverseProperty("DetalleComprobantes")]
    public Comprobante? Comprobante { get; set; }


    [ForeignKey("Cuenta")]
    public int IdCuenta { get; set; }

    [InverseProperty("DetalleComprobantes")]
    public Cuenta? Cuenta { get; set;}

}