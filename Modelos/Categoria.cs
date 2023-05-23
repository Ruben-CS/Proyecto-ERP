using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Modelos.Models;

namespace Modelos;

public class Categoria
{
    [Key]
    public int IdCategoria { get; set; }

    [Required]
    public string Nombre { get; set; }

    public string? Descripcion { get; set; }


    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public virtual Usuario? Usuario { get; set; }


    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    public virtual Empresa?
        Empresa
    {
        get;
        set;
    } // Propiedad de navegación para la relación con la tabla Empresa


    public int? IdCategoriaPadre { get; set; }

    public virtual Categoria?
        IdCategoriaPadreNavigation
    {
        get;
        set;
    } // Propiedad de navegación para la relación recursiva

    public bool             Estado         { get; set; } = true;
    public List<Categoria>? HijosCategoria { get; set; }

    public ICollection<ArticuloCategoria>? ArticuloCategorias { get; set; }
}