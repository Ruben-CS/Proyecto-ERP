using System.ComponentModel.DataAnnotations;

namespace ModuloContabilidadApi.Models;

public class Empresa
{
    [Key] public Guid IdEmpresa { get; set; } = Guid.NewGuid();

    [Required] public               string  Nombre    { get; set; }
    [Required] public               string  Nit       { get; set; }
    [Required] public               string  Sigla     { get; set; }
    public                          string? Telefono  { get; set; }
    [EmailAddress] public           string  Correo    { get; set; }
    public                          string? Direccion { get; set; }
    [Required] [Range(3, 7)] public int     Niveles   { get; set; }
    public                          object  IdUsuario { get; set; }
}