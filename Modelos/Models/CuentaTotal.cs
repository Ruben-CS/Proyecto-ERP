namespace Modelos.Models;

public class CuentaTotal
{
	public int IdCuenta { get; set; }

	public decimal  DebeTotal  { get; set; }
	public decimal? HaberTotal { get; set; }
	public decimal? DebeSaldo  { get; set; }
	public decimal? HaberSaldo { get; set; }

	public string NombreEmpresa { get; set; }
	public string NombreGestion { get; set; }
	public string NombreCuenta  { get; set; }

	public string NombreMoneda    { get; set; }
	public string NombreMonedaAlt { get; set; }
}