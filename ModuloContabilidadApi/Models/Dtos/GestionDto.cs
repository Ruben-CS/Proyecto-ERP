using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models.Dtos;

public class GestionDto
{
    public Guid IdGestion { get; set; }

    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosGestion Estado      { get; set; }
    public List<PeriodoDto>  Periodos    { get; set; }
    

    [ForeignKey("Empresa")]
    public Guid IdEmpresa { get; set; }

    [ForeignKey("Usuario")]
    public Guid IdUsuario { get; set; }

    [InverseProperty("Gestiones")]
    public Usuario Usuario { get; set; }
    
    [InverseProperty("Gestiones")]
    public EmpresaDto EmpresaDto { get; set; }
}