using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public sealed class ComprobanteDto
{
    public int IdComprobante { get; set; }

    public string            Serie  { get; set; } = null!;
    public string            Glosa  { get; set; } = null!;
    public DateTime          Fecha  { get; set; }
    public string            Tc     { get; set; } = null!;
    public EstadoComprobante Estado { get; set; } = EstadoComprobante.Abierto;

    public TipoComprobante TipoComprobante { get; set; }

    public List<DetalleComprobante>? DetalleComprobantes { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public int IdMoneda { get;   set; }
    public Moneda? Moneda { get; set;}
}