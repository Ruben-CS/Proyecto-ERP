using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ModuloContabilidadApi.Models;

public class Empresa
{
    [Key]
    public int IdEmpresa { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Nit { get; set; }

    [Required]
    public string Sigla { get; set; }

    public string? Telefono { get; set; }

    [EmailAddress]
    public string Correo { get; set; }

    public string? Direccion { get; set; }

    [Required]
    [Range(3, 7, ErrorMessage = "Nivel no aceptale")]
    public int Niveles { get; set; }

    public bool IsDeleted { get; set; } = false;

    public List<Gestion>? Gestiones { get; set; }

    // [ConcurrencyCheck]
    // public byte[]? TimeStamp { get; set; }

    [ForeignKey("Usuario")]
    public int? IdUsuario { get; set; }

    [InverseProperty("Empresas")]
    public Usuario? Usuario { get; set; }
}