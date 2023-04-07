using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Modelos.Models.Enums;
using ModuloContabilidadApi.Models.Dtos;

namespace Modelos.Models.Dtos;

public class GestionDto
{
    [Key]
    public int               IdGestion   { get; set; }
    public string            Nombre      { get; set; }
    public DateTime?          FechaInicio { get; set; }
    public DateTime?          FechaFin    { get; set; }
    public EstadosGestion    Estado      { get; set; } = EstadosGestion.Abierto;

    public List<PeriodoDto>? Periodos    { get; set; }

    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; } = 1;

    [InverseProperty("Gestiones")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("Gestiones")]
    public Empresa? EmpresaDto { get; set; }
}