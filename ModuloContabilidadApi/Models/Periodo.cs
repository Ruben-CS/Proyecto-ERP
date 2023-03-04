using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Periodo
{
    public Guid           IdPeriodo   { get; set; }
    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosPeriodo Estado      { get; set; } = EstadosPeriodo.Abierto;

    public Guid    IdUsuario { get; set; }
    public Guid    IdGestion { get; set; }
    public Usuario Usuario   { get; set; }
    public Gestion Gestion   { get; set; }
}