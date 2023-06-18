using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Enums;
using System.Net.Mime;

namespace ReportApi.Controllers
{
    public class EmpresaReporteControllerApi : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresaReporteControllerApi(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListarEmpresas")]
        [Produces(MediaTypeNames.Application.Xml)]
        public async Task<IActionResult> ListarEmpresasReporte()
        {
            try
            {
                var empresas     = await _context.Empresas.ToListAsync();
                var nuevaempresa = new List<EmpresaReporte>();

                foreach (var empresa in empresas)
                {
                    EmpresaReporte reporte = new EmpresaReporte();
                    reporte.Sigla       = empresa.Sigla;
                    reporte.RazonSocial = empresa.Nombre;
                    reporte.Nit         = empresa.Nit;
                    reporte.Telefono    = empresa.Telefono;
                    reporte.Correo      = empresa.Correo;

                    nuevaempresa.Add(reporte);
                }

                return Ok(nuevaempresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ListarGestiones/{IdEmpresa}")]
        [Produces(MediaTypeNames.Application.Xml)]
        public async Task<IActionResult> ListarGestiones([FromRoute] int IdEmpresa)
        {
            try
            {
                var gestiones = await _context.Gestiones
                                              .Where(x => x.IdEmpresa == IdEmpresa)
                                              .ToListAsync();
                List<GestionesReporte> nuevaempresa = new();
                var empresas = await _context.Empresas
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .ToListAsync();
                var nombreEmpresa = empresas[0].Nombre;
                foreach (var empresa in gestiones)
                {
                    GestionesReporte reporte = new()
                    {
                        NombreEmpresa = nombreEmpresa,
                        FechaFin      = empresa.FechaFin!.Value,
                        RazonSocial   = empresa.Nombre,
                        FechaInicio   = empresa.FechaInicio!.Value
                    };
                    if (empresa.Estado == EstadosGestion.Abierto)
                    {
                        reporte.Estado = "Abierta";
                    }
                    else
                    {
                        reporte.Estado = "Cerrada";
                    }

                    nuevaempresa.Add(reporte);
                }

                return Ok(nuevaempresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ListarPeriodos/{IdGestion}")]
        [Produces(MediaTypeNames.Application.Xml)]
        public async Task<IActionResult> ListarPeriodos([FromRoute] int IdGestion)
        {
            try
            {
                var gestiones = await _context.Periodos
                                              .Where(x => x.IdGestion == IdGestion)
                                              .ToListAsync();
                var ges = await _context.Gestiones.Where(x => x.IdGestion == IdGestion)
                                        .ToListAsync();
                var idempresa    = ges[0].IdEmpresa;
                var nuevaempresa = new List<PeriodosReporte>();
                var empresas = await _context.Empresas
                                             .Where(x => x.IdEmpresa == idempresa)
                                             .ToListAsync();
                var nombreEmpresa = empresas[0].Nombre;
                foreach (var empresa in gestiones)
                {
                    var reporte = new PeriodosReporte
                    {
                        NombreEmpresa = nombreEmpresa,
                        FechaFin      = empresa.FechaFin,
                        Periodo       = empresa.Nombre,
                        FechaInicio   = empresa.FechaInicio
                    };
                    if (empresa.Estado == EstadosPeriodo.Abierto)
                    {
                        reporte.Estado = "Abierto";
                    }
                    else
                    {
                        reporte.Estado = "Cerrado";
                    }
                    nuevaempresa.Add(reporte);
                }
                return Ok(nuevaempresa);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}