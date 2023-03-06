using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models.Dtos;

public class Periodo
{
    public Guid           IdPeriodo   { get; set; }
    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosPeriodo Estado      { get; set; } = EstadosPeriodo.Abierto;

    [ForeignKey("Usuario")]
    public Guid IdUsuario { get; set; }

    [ForeignKey("Gestion")]
    public Guid IdGestion { get; set; }

    [InverseProperty("Periodos")]
    public Usuario Usuario { get; set; }

    [InverseProperty("Periodos")]
    public Gestion Gestion { get; set; }
}