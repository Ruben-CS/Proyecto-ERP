using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Gestion
{
    [Key]
    public int IdGestion { get; set; }

    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosGestion Estado      { get; set; } = EstadosGestion.Abierto;

    public List<Periodo> Periodos { get; set; }

    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    [InverseProperty("Gestiones")]
    public Usuario Usuario { get; set; }

    [InverseProperty("Gestiones")]
    public Empresa Empresa { get; set; }
}