using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using System.Net.Mime;

namespace ReportApi.Controllers
{
    public class EmpresaReporteControllerApi : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresaReporteControllerApi(ApplicationDbContext context)
        {
            _context=context;
        }

        [HttpGet("ListarEmpresas")]
        [Produces(MediaTypeNames.Application.Xml)]
        public async Task<IActionResult> ListarEmpresasReporte()
        {
            try
            {
                var empresas = await _context.Empresas.ToListAsync();
                List<EmpresaReporte> nuevaempresa = new List<EmpresaReporte>();

                foreach(var empresa in empresas)
                {
                    EmpresaReporte reporte = new EmpresaReporte();
                    reporte.Sigla = empresa.Sigla;
                    reporte.RazonSocial = empresa.Nombre;
                    reporte.Nit = empresa.Nit;
                    reporte.Telefono = empresa.Telefono;
                    reporte.Correo = empresa.Correo;

                    nuevaempresa.Add(reporte);
                }

                return Ok(nuevaempresa);
            }
            catch(Exception)
            {
                throw;
            }

        }
    }
}
