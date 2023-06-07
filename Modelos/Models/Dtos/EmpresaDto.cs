using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Modelos.Models.Dtos;

public class EmpresaDto
{
    public int     IdEmpresa        { get; set; }
    public string  Nombre           { get; set; }
    public string  Nit              { get; set; }
    public string  Sigla            { get; set; }
    public string? Telefono         { get; set; }
    public string? Correo           { get; set; }
    public string? Direccion        { get; set; }
    public int     Niveles          { get; set; } = 3;
    public bool    IsDeleted        { get; set; }
    public int     IdUsuario        { get; set; }
    public bool?   TieneIntegracion { get; set; } = false;
    public int?    Cuenta1          { get; set; }
    public int?    Cuenta2          { get; set; }
    public int?    Cuenta3          { get; set; }
    public int?    Cuenta4          { get; set; }
    public int?    Cuenta5          { get; set; }
    public int?    Cuenta6          { get; set; }
    public int?    Cuenta7          { get; set; }
}