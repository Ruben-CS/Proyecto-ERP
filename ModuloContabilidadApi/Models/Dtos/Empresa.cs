using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuloContabilidadApi.Models.Dtos;

public class Empresa
{
    public Guid          IdEmpresa { get; set; } = Guid.NewGuid();
    public string        Nombre    { get; set; }
    public string        Nit       { get; set; }
    public string        Sigla     { get; set; }
    public string?       Telefono  { get; set; }
    public string        Correo    { get; set; }
    public string?       Direccion { get; set; }
    public int           Niveles   { get; set; }
    public bool          IsDeleted { get; set; } = false;
    public List<Gestion> Gestiones { get; set; }
    
    [ForeignKey("Usuario")]
    public Guid UsuarioId { get; set; }
    [InverseProperty("Empresas")]
    public Usuario Usuario { get; set; }
}