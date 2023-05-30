using System.ComponentModel.DataAnnotations;
using Modelos.Models;

namespace Modelos.Models;


public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public string Nombre { get; set; }

    public string Password { get; set; }

    public List<Empresa>? Empresas  { get; set; }

    public string Tipo { get; set; } = "admin";
    public List<Gestion>? Gestiones { get; set; }
    public List<Periodo>? Periodos  { get; set; }

    public List<Articulo>?  Articulos       { get; set; }
    public List<Nota>?      Notas           { get; set; }
    public List<Categoria>? HijosCategorias { get; set; }

    public List<EmpresaMoneda>?      EmpresaMonedas      { get; set; }
    public List<Comprobante>?        Comprobantes        { get; set; }
    public List<DetalleComprobante>? DetalleComprobantes { get; set; }
    public List<Moneda>?             Monedas             { get; set; }

}