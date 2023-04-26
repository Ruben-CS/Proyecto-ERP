using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ModuloContabilidadApi.Models;

namespace Modelos.Models.Dtos;

public class EmpresaDto
{
    public int IdEmpresa { get; set; }

    public string Nombre { get; set; }

    public string Nit { get; set; }

    public string Sigla { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public int Niveles { get; set; } = 3;

    public bool IsDeleted { get; set; } = false;

    public List<Gestion>? Gestiones { get; set; }

    public List<EmpresaMoneda>? EmpresaMonedas { get; set; }

    public List<Cuenta>? Cuentas { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }
}