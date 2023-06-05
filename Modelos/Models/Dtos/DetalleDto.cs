namespace Modelos.Models.Dtos;

public class DetalleDto
{
    public int     IdArticulo  { get; set; }
    public int     NroLote     { get; set; }
    public int     IdNota      { get; set; }
    public int     Cantidad    { get; set; }
    public decimal PrecioVenta { get; set; }
}