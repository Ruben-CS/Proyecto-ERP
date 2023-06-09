using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Categoria;

public class TreeItemDataCategoria
{
    public int    IdCategoria { get; }
    public string Nombre      { get; set; }

    public int? IdCategoriaPadre { get; }

    public string?                        Descripcion  { get; }
    public HashSet<TreeItemDataCategoria> CuentasHijas { get; set; }

    public TreeItemDataCategoria(CategoriaDto categoriaDto)
    {
        IdCategoria      = categoriaDto.IdCategoria;
        Nombre           = categoriaDto.Nombre;
        Descripcion      = categoriaDto.Descripcion;
        IdCategoriaPadre = categoriaDto.IdCategoriaPadre;
        CuentasHijas     = new HashSet<TreeItemDataCategoria>();
    }
}