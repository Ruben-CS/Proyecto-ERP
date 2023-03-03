using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ModuloContabilidadApi.Models.Enums;

namespace ModuloContabilidadApi.Models;

public class Gestion
{
    /*
        IdUsuario
        IdEmpresa
     * 
     */
    [Key] public Guid     IdGestion   { get; set; } = Guid.NewGuid();
    public       string   Nombre      { get; set; }
    public       DateTime FechaInicio { get; set; }
    public       DateTime FechaFin    { get; set; }
    public EstadosGestion Estado { get; set; } = EstadosGestion.Abierto;
}