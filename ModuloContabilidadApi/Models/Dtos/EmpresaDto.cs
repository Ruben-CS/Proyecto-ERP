using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloContabilidadApi.Models.Dtos;

public class EmpresaDto
{
    public int IdEmpresa { get; set; }

    [Required(ErrorMessage = "Empresa requiere un nombre")]
    string Nombre { get; set; }

    [Required(ErrorMessage = "Empresa requiere un NIT")]
    public string Nit { get; set; }

    [Required(ErrorMessage = "Empresa requiere una sigla")]

    public string Sigla { get; set; }

    public string? Telefono  { get; set; }
    [EmailAddress]
    public string  Correo    { get; set; }
    public string? Direccion { get; set; }
    public int     Niveles   { get; set; }
    public bool    IsDeleted { get; set; } = false;

    public List<Gestion>? Gestiones { get; set; }
    // public byte[]         TimeStamp { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    [InverseProperty("Empresas")]
    public Usuario? Usuario { get; set; }
}