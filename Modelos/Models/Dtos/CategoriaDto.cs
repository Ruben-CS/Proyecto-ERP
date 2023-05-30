namespace Modelos.Models.Dtos;

public class CategoriaDto
{
    public int     IdCategoria      { get; set; }
    public string  Nombre           { get; set; }
    public string? Descripcion      { get; set; }
    public int     IdUsuario        { get; set; }
    public int     IdEmpresa        { get; set; }
    public int?    IdCategoriaPadre { get; set; }
    public bool    Estado           { get; set; }
}