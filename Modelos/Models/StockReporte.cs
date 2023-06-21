namespace Modelos.Models;

public class StockReporte
{
    public int IdCategoria { get; set; }

    public string? NombreArticulo { get; set; }

    public decimal Precio        { get; set; }
    public int?    StockArticulo { get; set; }
    public string? Estado        { get; set; }

    public string NombreEmpresa { get; set; }
    public string NombreGestion { get; set; }
    public string NombreCuenta  { get; set; }
}