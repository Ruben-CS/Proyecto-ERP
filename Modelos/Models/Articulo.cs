using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class Articulo
{
    [Key]
    public int IdArticulo { get; set; }

    public string? Nombre      { get; set; }
    public string? Descripcion { get; set; }
    public int     Cantidad    { get; set; }
    public decimal   PrecioVenta { get; set; }

    [ForeignKey("Usuario")]
    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }


    [ForeignKey("Empresa")]
    public int IdEmpresa { get; set; }

    public Empresa? Empresa { get; set; }

    public ICollection<ArticuloCategoria>? ArticuloCategorias { get; set; }

    public Articulo()
    {
        ArticuloCategorias = new List<ArticuloCategoria>();
    }
}