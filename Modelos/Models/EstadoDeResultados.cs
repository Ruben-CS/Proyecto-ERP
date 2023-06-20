namespace Modelos.Models;

public class EstadoDeResultados
{
    public decimal         TotalIngresos   { get; set; }
    public decimal         TotalCostos     { get; set; }
    public decimal         TotalGastos     { get; set; }
    public string        NombreEmpresa   { get; set; }
    public string        NombreGestion   { get; set; }
    public string?       NombreCuenta    { get; set; }
    public string?       NombreCuentaC   { get; set; }
    public string?       NombreCuentaG   { get; set; }
    public string        NombreMoneda    { get; set; }
    public string        NombreMonedaAlt { get; set; }
    public int           IdCuenta        { get; set; }
    public List<Ingreso> Ingresos        { get; set; } = new List<Ingreso>();
    public List<Costo>   Costos          { get; set; } = new List<Costo>();
    public List<Gasto>   Gastos          { get; set; } = new List<Gasto>();

}