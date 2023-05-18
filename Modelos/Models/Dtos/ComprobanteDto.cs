using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public sealed class ComprobanteDto
{
    public int IdComprobante { get; set; }

    public int?              Serie  { get; set; }
    public string            Glosa  { get; set; } = null!;
    public DateTime          Fecha  { get; set; }
    public decimal?          Tc     { get; set; }
    public EstadoComprobante Estado { get; set; } = EstadoComprobante.Abierto;

    public TipoComprobante TipoComprobante { get; set; }

    public List<DetalleComprobante>? DetalleComprobantes { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public int      IdEmpresa { get; set; }
    public Empresa? Empresa   { get; set; }

    public int     IdMoneda { get; set; }
    public Moneda? Moneda   { get; set; }
}