namespace Modelos.Models.Dtos;

public class ArticuloDto
{
    public int     IdArticulo  { get; set; }
    public string? Nombre      { get; set; }
    public string? Descripcion { get; set; }
    public int     Cantidad    { get; set; }
    public decimal PrecioVenta { get; set; }
    public int     IdUsuario   { get; set; }
    public int     IdEmpresa   { get; set; }
}