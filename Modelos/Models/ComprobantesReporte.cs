using Modelos.Models.Enums;

namespace Modelos.Models;

public class ComprobantesReporte
{
    public string            NombreEmpresa    { get; set; }
    public string            NombreMoneda     { get; set; }
    public int?              Serie            { get; set; }
    public DateTime?         Fecha            { get; set; }
    public TipoComprobante   TipoComprobante  { get; set; }
    public decimal?          TC               { get; set; }
    public string            IdMoneda         { get; set; }
    public string?           GlosaComprobante { get; set; }
    public EstadoComprobante Estado           { get; set; }
    public string?           NombreCuenta     { get; set; }
    public string?           Glosa            { get; set; }
    public decimal?          MontoDebe        { get; set; }
    public decimal?          MontoHaber       { get; set; }

    public List<DetallesComprobantesReporte> detallesComprobantesReportes { get; set; } =
        new();
}