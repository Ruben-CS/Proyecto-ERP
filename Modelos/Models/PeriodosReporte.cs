namespace Modelos.Models;

public class PeriodosReporte
{
    public string   NombreEmpresa { get; set; }
    public DateTime FechaInicio   { get; set; }
    public DateTime FechaFin      { get; set; }
    public string   Periodo       { get; set; }
    public string   Estado        { get; set; }
}