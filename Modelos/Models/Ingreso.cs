namespace Modelos.Models;

public class Ingreso
{
    public string NombreCuenta { get; set; }
    public decimal  TotalDebe    { get; set; }
    public decimal  TotalHaber   { get; set; }
    public decimal  TotalIngreso { get; set; }

}