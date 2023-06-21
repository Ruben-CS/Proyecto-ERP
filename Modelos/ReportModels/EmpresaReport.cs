namespace Modelos.ReportModels;

public sealed class EmpresaReport
{
    public string Sigla       { get; set; }
    public string RazonSocial { get; set; }
    public string Nit         { get; set; }
    public string? Telefono    { get; set; }
    public string? Correo      { get; set; }
}