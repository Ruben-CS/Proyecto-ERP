namespace Modelos.Models.Dtos;

public class LoteDto
{
    public int      IdLote           { get; set; }
    public DateTime FechaIngreso     { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int      Cantidad         { get; set; }
    public decimal  PrecioCompra     { get; set; }
    public int      Stock            { get; set; }
    public int      IdArticulo       { get; set; }
    public int      IdNota           { get; set; }
}