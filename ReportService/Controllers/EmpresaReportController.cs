using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.ReportModels;

namespace ReportService.Controllers;

[ApiController]
[Route("empresareport")]
public class EmpresaReportController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public EmpresaReportController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("ListarEmpresas")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> ListarEmpresas()
    {
        var empresas = await _dbContext.Empresas.ToListAsync();
        var nuevaempresa = empresas.Select(empresa => new EmpresaReport
                                   {
                                       Sigla       = empresa.Sigla,
                                       RazonSocial = empresa.Nombre,
                                       Nit         = empresa.Nit,
                                       Telefono    = empresa.Telefono,
                                       Correo      = empresa.Correo
                                   })
                                   .ToList();
        return Ok(nuevaempresa);
    }
}