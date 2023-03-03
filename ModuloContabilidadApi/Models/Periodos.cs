using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Periodos
{
    /*
     * IdPeriodo
        Nombre
        FechaInicio
        FechaFin
        Estado
        IdUsuario
        IdGestion
     */
    public Guid           IdPeriodo   { get; set; } = Guid.NewGuid();
    public string         Nombre      { get; set; }
    public DateTime       FechaInicio { get; set; }
    public DateTime       FechaFin    { get; set; }
    public EstadosPeriodo Estado      { get; set; } = EstadosPeriodo.Abierto;
    
}