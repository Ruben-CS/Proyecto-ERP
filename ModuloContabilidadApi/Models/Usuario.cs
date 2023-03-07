using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace ModuloContabilidadApi.Models;

public class Usuario
{
    [Key]
    public Guid IdUsuario { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; }

    public string Password { get; set; }

    public List<Empresa>? Empresas  { get; set; }
    public List<Gestion>? Gestiones { get; set; }
    public List<Periodo>? Periodos  { get; set; }
}