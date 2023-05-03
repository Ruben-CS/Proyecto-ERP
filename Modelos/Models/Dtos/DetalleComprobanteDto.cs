namespace Modelos.Models.Dtos;

public sealed class DetalleComprobanteDto
{
    public int IdDetalleComprobante { get; set; }

    public int    Numero        { get; set; }
    public string Glosa         { get; set; }
    public float  MontoDebe     { get; set; }
    public float  MontoHaber    { get; set; }
    public float  MontoDebeAlt  { get; set; }
    public float  MontoHaberAlt { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public int IdComprobante { get; set; }

    public Comprobante? Comprobante { get; set; }

    public int IdCuenta { get; set; }

    public Cuenta? Cuenta { get; set;}
}