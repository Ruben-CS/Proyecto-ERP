using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;
using ModuloContabilidadApi.Models.Dtos;

namespace Modelos.Models.Dtos;

public class GestionDto
{
    public int IdGestion { get; set; }

    public string           Nombre      { get; set; }
    public DateTime         FechaInicio { get; set; }
    public DateTime         FechaFin    { get; set; }
    public EstadosGestion   Estado      { get; set; }
    public List<PeriodoDto> Periodos    { get; set; }


    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    [InverseProperty("Gestiones")]
    public Usuario Usuario { get; set; }

    [InverseProperty("Gestiones")]
    public EmpresaDto EmpresaDto { get; set; }
}