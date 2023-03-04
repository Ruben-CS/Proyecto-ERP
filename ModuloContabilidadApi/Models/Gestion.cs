using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Gestion
{
    [Key]
    public Guid IdGestion { get; set; }

    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosGestion Estado      { get; set; } = EstadosGestion.Abierto;

    public List<Periodo> Periodos  { get; set; }
    public Guid           IdEmpresa { get; set; }
    public Guid           IdUsuario { get; set; }
    public Usuario        Usuario   { get; set; }
    public Empresa        Empresa   { get; set; }
}