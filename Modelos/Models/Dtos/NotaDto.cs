using Modelos.Models.Enums;

namespace Modelos.Models.Dtos;

public class NotaDto
{
    public int        IdNota        { get; set; }
    public int?        NroNota       { get; set; }
    public DateTime   Fecha         { get; set; }
    public string     Descripcion   { get; set; }
    public float      Total         { get; set; }
    public TipoNota   TipoNota      { get; set; }
    public int        IdUsuario     { get; set; }
    public int        IdEmpresa     { get; set; }
    public int        IdComprobante { get; set; }
    public EstadoNota EstadoNota    { get; set; }
}