using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ModuloContabilidadApi.Models;

public class Usuario : IdentityUser
{
    [Key]
    public Guid IdUsuario { get; set; }

    public string Nombre   { get; set; }
    public string Password { get; set; }

    public List<Empresa>  Empresas  { get; set; }
    public List<Gestion>  Gestiones { get; set; }
    public List<Periodo> Periodos  { get; set; }
}