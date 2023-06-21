namespace Modelos.Models;

public class LibroDiarioReporte
{
    public int       IdCuenta { get; set; }
    public DateTime? Fecha    { get; set; }
    public string?   Glosa    { get; set; }

    public decimal  Debe          { get; set; }
    public decimal? Haber         { get; set; }
    public string NombreEmpresa { get; set; }
    public string NombrePeriodo { get; set; }
    public string NombreGestion { get; set; }
    public string NombreCuenta  { get; set; }

    public string NombreMoneda    { get; set; }
    public string NombreMonedaAlt { get; set; }

}