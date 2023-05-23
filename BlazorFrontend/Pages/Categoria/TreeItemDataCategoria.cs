using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Categoria;

public class TreeItemDataCategoria
{
    public int    IdCategoria { get; set; }
    public string Nombre      { get; set; }

    public int? IdCategoriaPadre { get; set; }

    public string?                        Descripcion  { get; set; }
    public HashSet<TreeItemDataCategoria> CuentasHijas { get; set; }

    private Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>> RootItems
    {
        get;
        set;
    }

    private TreeItemDataCategoria SelectedValue { get; set; }

    public TreeItemDataCategoria(CategoriaDto categoriaDto)
    {
        IdCategoria      = categoriaDto.IdCategoria;
        Nombre           = categoriaDto.Nombre;
        Descripcion      = categoriaDto.Descripcion;
        IdCategoriaPadre = categoriaDto.IdCategoriaPadre;
        CuentasHijas     = new HashSet<TreeItemDataCategoria>();
    }
}