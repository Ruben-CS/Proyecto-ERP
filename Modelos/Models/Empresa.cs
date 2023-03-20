using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModuloContabilidadApi.Models;

namespace Modelos.Models;

public class Empresa
{
    [Key]
    public int IdEmpresa { get; set; }

    [Required(ErrorMessage = "Empresa requiere un nombre")]
    public string Nombre { get; set; }

    [Required (ErrorMessage = "Empresa requiere un NIT")]
    public string Nit { get; set; }

    [Required (ErrorMessage = "Empresa requiere una sigla")]
    public string Sigla { get; set; }

    public string? Telefono { get; set; }

    [EmailAddress (ErrorMessage = "Correo no valido")]
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