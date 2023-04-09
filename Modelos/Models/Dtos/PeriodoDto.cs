using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models.Enums;


namespace Modelos.Models.Dtos;

public class PeriodoDto
{
    public int            IdPeriodo   { get; set; }
    public string         Nombre      { get; set; }
    public DateTime?       FechaInicio { get; set; }
    public DateTime?       FechaFin    { get; set; }
    public EstadosPeriodo Estado      { get; set; } = EstadosPeriodo.Abierto;

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; } = 1;

    [ForeignKey("Gestion")]
    public int IdGestion { get; set; }

    [InverseProperty("Periodos")]
    public Usuario? Usuario { get; set; }

    [InverseProperty("Periodos")]
    public GestionDto? GestionDto { get; set; }
}