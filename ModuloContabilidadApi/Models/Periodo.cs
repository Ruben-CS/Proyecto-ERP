using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Periodo
{
    [Key]
    public int IdPeriodo { get; set; }

    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosPeriodo Estado      { get; set; } = EstadosPeriodo.Abierto;

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    [ForeignKey("Gestion")]
    public int IdGestion { get; set; }

    [InverseProperty("Periodos")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("Periodos")]
    public Gestion? Gestion { get; set; }
}