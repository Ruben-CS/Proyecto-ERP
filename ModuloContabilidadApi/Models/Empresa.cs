using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloContabilidadApi.Models;

public class Empresa
{
    [Key]
    public Guid IdEmpresa { get; set; } = Guid.NewGuid();

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
    [Range(3, 7,ErrorMessage = "Nivel no aceptale")]
    public int Niveles { get; set; }

    public bool   IsDeleted { get; set; } = false;

    public List<Gestion> Gestiones { get; set; }
    
    [ForeignKey("Usuario")]
    public Guid    UsuarioId { get; set; }
    [InverseProperty("Empresas")]
    public Usuario Usuario   { get; set; }
    
}