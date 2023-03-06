using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ModuloContabilidadApi.Models;

public class Usuario : IdentityUser
{
    [Key]
    public Guid IdUsuario { get; set; } = Guid.NewGuid();

    public string Nombre   { get; set; }
    public string Password { get; set; }

    #region propiedades de IdentityServer

    public bool? EmailConfirmed       { get; set; }
    public bool? TwoFactorEnabled     { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
    public bool? LockoutEnabled       { get; set; }

    public int? AccessFailedCount { get; set; }

    #endregion

    public List<Empresa> Empresas  { get; set; }
    public List<Gestion> Gestiones { get; set; }
    public List<Periodo> Periodos  { get; set; }
}