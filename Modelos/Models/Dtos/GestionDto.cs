using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Modelos.Models.Enums;
using ModuloContabilidadApi.Models.Dtos;

namespace Modelos.Models.Dtos;

public class GestionDto
{
    public int IdGestion { get; set; }
    [JsonPropertyName("nombre")]

    public string           Nombre      { get; set; }
    [JsonPropertyName("fechaInicio")]
    public DateTime         FechaInicio { get; set; }
    [JsonPropertyName("fechaFin")]
    public DateTime         FechaFin    { get; set; }
    [JsonPropertyName("estado")]
    public EstadosGestion   Estado      { get; set; }
    [JsonPropertyName("periodos")]
    public List<PeriodoDto>? Periodos { get; set; }

    [ForeignKey("Empresa")]
    [JsonPropertyName("empresa")]

    public int IdEmpresa { get; set; }


    [ForeignKey("Usuario")]
    [JsonPropertyName("usuario")]
    public int IdUsuario { get; set; } = 1;

    [InverseProperty("Gestiones")]
    [JsonPropertyName("usuario")]
    public Usuario Usuario { get; set; }

    [InverseProperty("Gestiones")]
    [JsonPropertyName("empresa")]
    public EmpresaDto EmpresaDto { get; set; }
}