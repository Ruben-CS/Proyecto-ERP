namespace Modelos.Models.Dtos;

public sealed class ComprobanteDto
{
    public int IdComprobante { get; set; }

    public string Serie { get; set; }
    public string Glosa { get; set; }
    public DateTime Fecha { get;         set; }
    public string TC              { get; set; }
    public bool   Estado          { get; set; }
    public string TipoComprobante { get; set; }

    public List<DetalleComprobante>? DetalleComprobantes { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public int IdMoneda { get;   set; }
    public Moneda? Moneda { get; set;}
}