using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ModuloContabilidadApi.Models;

public class Gestion
{
    /*
     *IdGestion
        Nombre
        FechaInicio
        FechaFin
        Estado
        IdUsuario
        IdEmpresa
     * 
     */
    [Key] public Guid IdGestion { get; set; } = Guid.NewGuid();
    public       string    Nombre         { get; set; }
    
}

