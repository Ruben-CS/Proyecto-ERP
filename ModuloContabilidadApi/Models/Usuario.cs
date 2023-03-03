using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ModuloContabilidadApi.Models;

public class Usuario : IdentityUser
{
    [Key] public string IdUsuario { get; set; }
    public       string Nombre    { get; set; }
    public       string Password  { get; set; }
    
}