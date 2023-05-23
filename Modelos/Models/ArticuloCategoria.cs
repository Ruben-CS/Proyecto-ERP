using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos.Models;

public class ArticuloCategoria
{
    [Key, Column(Order = 0)]
    public int IdArticulo { get; set; }

    [Key, Column(Order = 1)]
    public int    IdCategoria     { get; set; }
    public string NombreCategoria { get; set; }

    // Propiedades de navegación
    public Articulo?  Articulo  { get; set; }
    public Categoria? Categoria {get; set;}

}