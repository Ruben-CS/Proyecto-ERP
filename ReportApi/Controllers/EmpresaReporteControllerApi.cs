using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Enums;
using System.Net.Mime;
using Microsoft.OpenApi.Extensions;

namespace ReportApi.Controllers;

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
                var reporte = new EmpresaReporte();
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
                    reporte.Estado = "Abierta";
                else
                    reporte.Estado = "Cerrada";

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
            var ges = await _context
                            .Gestiones.Where(x => x.IdGestion == IdGestion)
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
                    reporte.Estado = "Abierto";
                else
                    reporte.Estado = "Cerrado";

                nuevaempresa.Add(reporte);
            }

            return Ok(nuevaempresa);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("ListarComprobantes/{IdComprobante}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> ListarComprobantes(
        [FromRoute] int IdComprobante)
    {
        try
        {
            var comprobantes = await _context.Comprobantes
                                             .Where(x =>
                                                 x.IdComprobante ==
                                                 IdComprobante)
                                             .ToListAsync();
            var detalleComprobante = await _context.DetalleComprobantes
                                                   .Where(x =>
                                                       x.IdComprobante ==
                                                       IdComprobante)
                                                   .ToListAsync();
            var idempresa    = comprobantes[0].IdEmpresa;
            var nuevaempresa = new List<ComprobantesReporte>();
            var empresas = await _context.Empresas
                                         .Where(x => x.IdEmpresa == idempresa)
                                         .ToListAsync();
            var nombreEmpresa = empresas[0].Nombre;

            foreach (var comprob in comprobantes)
            {
                var idmoneda = comprob.IdMoneda;
                var monedas = await _context.Monedas
                                            .Where(x => x.IdMoneda == idmoneda)
                                            .ToListAsync();
                var c = new ComprobantesReporte
                {
                    NombreEmpresa    = nombreEmpresa,
                    Serie            = comprob.Serie,
                    Fecha            = comprob.Fecha,
                    Estado           = comprob.Estado,
                    TipoComprobante  = comprob.TipoComprobante,
                    TC               = comprob.Tc,
                    NombreMoneda     = monedas[0].Nombre,
                    GlosaComprobante = comprob.Glosa
                };
                nuevaempresa.Add(c);
            }

            foreach (var d in detalleComprobante.Select(comprob =>
                         new DetallesComprobantesReporte
                         {
                             NombreCuenta = comprob.NombreCuenta,
                             Glosa        = comprob.Glosa,
                             MontoDebe    = comprob.MontoDebe,
                             MontoHaber   = comprob.MontoHaber
                         }))
                nuevaempresa.Last().detallesComprobantesReportes.Add(d);

            return Ok(nuevaempresa);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("ListarNotaDeCompraXML/{IdNota}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> ListarNotaDeCompraXML(
        [FromRoute] int IdNota)
    {
        try
        {
            var nota = await _context.Nota
                                     .Include(n => n.Lotes)
                                     .FirstOrDefaultAsync(n =>
                                         n.IdNota == IdNota);
            var notaIdEmpresa = nota.IdEmpresa;
            var notaEstado    = nota.EstadoNota;
            var empresa = await _context.Empresas
                                        .Where(x =>
                                            x.IdEmpresa == notaIdEmpresa)
                                        .ToListAsync();
            var nEmpresa = empresa[0].Nombre;
            if (nota is null) return StatusCode(404);

            var reporteNc = new List<ReporteNotaVenta>();

            foreach (var lote in nota.Lotes)
            {
                var reporte = new ReporteNotaVenta()
                {
                    Subtotal       = lote.Cantidad * lote.PrecioCompra,
                    Estado         = notaEstado,
                    NombreEmpresa  = nEmpresa,
                    IdNota         = nota.IdNota,
                    NroNota        = nota.NroNota!.Value,
                    Descripcion    = nota.Descripcion,
                    Fecha          = nota.Fecha,
                    Total          = nota.Total,
                    Tipo           = TipoNota.Compra,
                    NombreArticulo = await GetAritculoName(lote.IdArticulo),
                    Cantidad       = lote.Cantidad,
                    Precio         = lote.PrecioCompra,
                    NroLoteOficial = lote.NroLote!.Value
                };
                reporteNc.Add(reporte);
            }

            {
                return Ok(reporteNc);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //extras
    [HttpGet("ListarNotaDeVentaXML/{IdNota}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> ListarNotaDeVentaXML(
        [FromRoute] int IdNota)
    {
        try
        {
            var detalleVenta = await _context.Detalle
                                             .Where(d => d.IdNota == IdNota)
                                             .ToListAsync();
            var nota = await _context.Nota.Where(x => x.IdNota == IdNota)
                                     .FirstOrDefaultAsync();
            //extras
            var notaIdEmpresa = nota.IdEmpresa;
            var notaEstado    = nota.EstadoNota;
            var empresa = await _context
                                .Empresas.Where(x =>
                                    x.IdEmpresa == notaIdEmpresa)
                                .ToListAsync();
            var nEmpresa = empresa[0].Nombre;


            var reporteNV = new List<ReporteNotaVenta>();

            foreach (var detalle in detalleVenta)
            {
                var reporte = new ReporteNotaVenta
                {
                    Subtotal       = detalle.Cantidad * detalle.PrecioVenta,
                    Estado         = notaEstado,
                    NombreEmpresa  = nEmpresa,
                    IdNota         = detalle.IdNota,
                    NroNota        = nota.NroNota!.Value,
                    Descripcion    = nota.Descripcion,
                    Fecha          = nota.Fecha,
                    Total          = nota.Total,
                    Tipo           = TipoNota.Venta,
                    NombreArticulo = await GetAritculoName(detalle.IdArticulo),
                    Cantidad       = detalle.Cantidad,
                    Precio         = detalle.PrecioVenta,
                    NroLoteOficial = detalle.NroLote
                };
                reporteNV.Add(reporte);
            }

            {
                return Ok(reporteNV);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("SumasYSaldosPrincipal/{IdGestion}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> SumasYSaldosPrincipal(
        [FromRoute] int IdGestion)
    {
        try
        {
            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;
            var comprobantes = await _context.Comprobantes
                                             .Include(
                                                 c => c.DetalleComprobantes)
                                             .Where(x =>
                                                 x.IdEmpresa == IdEmpresa)
                                             .OrderByDescending(e => e.Serie)
                                             .ToListAsync();
            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            // Crear una lista para almacenar los totales de cada cuenta
            var cuentasTotales = new List<CuentaTotal>();

            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var NGestion = gestion.Nombre;
                    foreach (var c in comprobantes)
                    {
                        var NMoneda  = await GetMonedaName(c.IdMoneda);
                        var NEmpresa = await GetEmpresaName(c.IdEmpresa);

                        if (c.Estado == EstadoComprobante.Abierto &&
                            c.Fecha  >= p.FechaInicio             &&
                            c.Fecha  <= p.FechaFin)
                        {
                            foreach (var detalle in c.DetalleComprobantes)
                            {
                                var cuentaExistente =
                                    cuentasTotales.FirstOrDefault(ct =>
                                        ct.IdCuenta == detalle.IdCuenta);
                                if (cuentaExistente != null)
                                {
                                    cuentaExistente.DebeTotal +=
                                        detalle.MontoDebe;
                                    cuentaExistente.HaberTotal +=
                                        detalle.MontoHaber;

                                    var diferencia = cuentaExistente.DebeTotal -
                                                     cuentaExistente.HaberTotal;
                                    cuentaExistente.DebeSaldo =
                                        diferencia > 0 ? diferencia : 0;
                                    cuentaExistente.HaberSaldo =
                                        diferencia < 0 ? -diferencia : 0;
                                    cuentaExistente.NombreGestion = NGestion;
                                    cuentaExistente.NombreEmpresa = NEmpresa;
                                    cuentaExistente.NombreMoneda  = NMoneda;
                                }
                                else
                                {
                                    var cuenta =
                                        await _context.Cuentas.FindAsync(
                                            detalle.IdCuenta);
                                    var diferencia = detalle.MontoDebe -
                                                     detalle.MontoHaber;

                                    cuentasTotales.Add(new CuentaTotal
                                    {
                                        IdCuenta   = (int)detalle.IdCuenta,
                                        DebeTotal  = detalle.MontoDebe,
                                        HaberTotal = detalle.MontoHaber,
                                        NombreCuenta = cuenta.Codigo + '-' +
                                                       cuenta?.Nombre,
                                        DebeSaldo = diferencia > 0
                                            ? diferencia
                                            : 0,
                                        HaberSaldo = diferencia < 0
                                            ? -diferencia
                                            : 0,
                                        NombreGestion = NGestion,
                                        NombreEmpresa = NEmpresa,
                                        NombreMoneda  = NMoneda
                                    });
                                }
                            }
                        }
                    }
                }
            }

            if (cuentasTotales.Count > 0)
            {
                return Ok(cuentasTotales);
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("SumasYSaldosAlternativa/{IdGestion}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> SumasYSaldosAlternativa([FromRoute] int IdGestion)
    {
        try
        {
            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;
            var comprobantes = await _context.Comprobantes
                                             .Include(
                                                 c => c.DetalleComprobantes)
                                             .Where(x =>
                                                 x.IdEmpresa == IdEmpresa)
                                             .OrderByDescending(e => e.Serie)
                                             .ToListAsync();
            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            // Crear una lista para almacenar los totales de cada cuenta
            var cuentasTotales = new List<CuentaTotal>();

            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var NGestion = gestion.Nombre;
                    foreach (var c in comprobantes)
                    {
                        var NMoneda  = await GetMonedaName(c.IdMoneda);
                        var NEmpresa = await GetEmpresaName(c.IdEmpresa);

                        if (c.Estado == EstadoComprobante.Abierto &&
                            c.Fecha  >= p.FechaInicio             &&
                            c.Fecha  <= p.FechaFin)
                        {
                            foreach (var detalle in c.DetalleComprobantes)
                            {
                                var cuentaExistente =
                                    cuentasTotales.FirstOrDefault(ct =>
                                        ct.IdCuenta == detalle.IdCuenta);
                                if (cuentaExistente != null)
                                {
                                    cuentaExistente.DebeTotal +=
                                        detalle.MontoDebeAlt;
                                    cuentaExistente.HaberTotal +=
                                        detalle.MontoHaberAlt;

                                    var diferencia = cuentaExistente.DebeTotal -
                                                     cuentaExistente.HaberTotal;
                                    cuentaExistente.DebeSaldo =
                                        diferencia > 0 ? diferencia : 0;
                                    cuentaExistente.HaberSaldo =
                                        diferencia < 0 ? -diferencia : 0;
                                    cuentaExistente.NombreGestion = NGestion;
                                    cuentaExistente.NombreEmpresa = NEmpresa;
                                    cuentaExistente.NombreMoneda  = NMoneda;
                                }
                                else
                                {
                                    var cuenta =
                                        await _context.Cuentas.FindAsync(
                                            detalle.IdCuenta);
                                    var diferencia = detalle.MontoDebeAlt -
                                                     detalle.MontoHaberAlt;

                                    cuentasTotales.Add(new CuentaTotal
                                    {
                                        IdCuenta   = detalle.IdCuenta,
                                        DebeTotal  = detalle.MontoDebeAlt,
                                        HaberTotal = detalle.MontoHaberAlt,
                                        NombreCuenta = cuenta.Codigo + '-' +
                                                       cuenta.Nombre,
                                        DebeSaldo = diferencia > 0
                                            ? diferencia
                                            : 0,
                                        HaberSaldo = diferencia < 0
                                            ? -diferencia
                                            : 0,
                                        NombreGestion = NGestion,
                                        NombreEmpresa = NEmpresa,
                                        NombreMoneda  = NMoneda
                                    });
                                }
                            }
                        }
                    }
                }
            }

            if (cuentasTotales.Count > 0)
            {
                return Ok(cuentasTotales);
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("EstadoDeResultadosPrincipal/{IdGestion}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> EstadoDeResultadosPrincipal(
        [FromRoute] int IdGestion)
    {
        try
        {
            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();

            var IdEmpresa = gestiones[0].IdEmpresa;

            var comprobantes = await _context.Comprobantes
                                             .Include(c => c.DetalleComprobantes)
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .OrderByDescending(e => e.Serie)
                                             .ToListAsync();

            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            EstadoDeResultados estadoDeResultado = new EstadoDeResultados();
            var                ingresosSet       = false;
            var                costosSet         = false;
            var                gastosSet         = false;
            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var NGestion = gestion.Nombre;
                    foreach (var c in comprobantes)
                    {
                        var NMoneda  = await GetMonedaName(c.IdMoneda);
                        var NEmpresa = await GetEmpresaName(c.IdEmpresa);
                        if (c.Estado == EstadoComprobante.Abierto &&
                            c.Fecha  >= p.FechaInicio             &&
                            c.Fecha  <= p.FechaFin)
                        {
                            foreach (var detalle in c.DetalleComprobantes)
                            {
                                var cuenta =
                                    await _context.Cuentas.FindAsync(detalle.IdCuenta);

                                var esIngreso = false;
                                var esCostos  = false;
                                var esGastos  = false;
                                if (cuenta.Codigo.StartsWith("4"))
                                {
                                    esIngreso = true;
                                }
                                else if (cuenta.Codigo.StartsWith("5.1"))
                                {
                                    esCostos = true;
                                }
                                else if (cuenta.Codigo.StartsWith("5.2"))
                                {
                                    esGastos = true;
                                }

                                if (esIngreso || esCostos || esGastos)
                                {
                                    estadoDeResultado.NombreGestion = NGestion;
                                    estadoDeResultado.NombreEmpresa = NEmpresa;
                                    estadoDeResultado.NombreMoneda  = NMoneda;

                                    if (esIngreso)
                                    {
                                        var ingreso =
                                            estadoDeResultado.Ingresos.FirstOrDefault(i =>
                                                i.NombreCuenta == detalle.NombreCuenta);
                                        if (ingreso == null)
                                        {
                                            ingreso = new Ingreso()
                                                { NombreCuenta = detalle.NombreCuenta };
                                            estadoDeResultado.Ingresos.Add(ingreso);
                                        }

                                        ingreso.TotalDebe  += detalle.MontoDebe;
                                        ingreso.TotalHaber += detalle.MontoHaber;
                                        ingreso.TotalIngreso = ingreso.TotalHaber -
                                            ingreso.TotalDebe;
                                    }

                                    if (esCostos)
                                    {
                                        var costo =
                                            estadoDeResultado.Costos.FirstOrDefault(i =>
                                                i.NombreCuenta == detalle.NombreCuenta);
                                        if (costo == null)
                                        {
                                            costo = new Costo()
                                                { NombreCuenta = detalle.NombreCuenta };
                                            estadoDeResultado.Costos.Add(costo);
                                        }

                                        costo.TotalDebe  += detalle.MontoDebe;
                                        costo.TotalHaber += detalle.MontoHaber;
                                        costo.TotalCosto =
                                            costo.TotalDebe - costo.TotalHaber;
                                    }

                                    if (esGastos)
                                    {
                                        var gasto =
                                            estadoDeResultado.Gastos.FirstOrDefault(i =>
                                                i.NombreCuenta == detalle.NombreCuenta);
                                        if (gasto == null)
                                        {
                                            gasto = new Gasto()
                                                { NombreCuenta = detalle.NombreCuenta };
                                            estadoDeResultado.Gastos.Add(gasto);
                                        }

                                        gasto.TotalDebe  += detalle.MontoDebe;
                                        gasto.TotalHaber += detalle.MontoHaber;
                                        gasto.TotalGasto =
                                            gasto.TotalDebe - gasto.TotalHaber;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (estadoDeResultado != null)
            {
                estadoDeResultado.TotalIngresos =
                    estadoDeResultado.Ingresos.Sum(i => i.TotalIngreso);
                estadoDeResultado.TotalGastos =
                    estadoDeResultado.Gastos.Sum(g => g.TotalGasto);
                estadoDeResultado.TotalCostos =
                    estadoDeResultado.Costos.Sum(c => c.TotalCosto);
                return Ok(estadoDeResultado);
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("LibroDiarioPrincipal/{IdGestion}/{IdPeriodo}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> LibroDiarioPrincipal(
        [FromRoute] int IdGestion, [FromRoute] int? IdPeriodo)
    {
        try
        {
            decimal totalDebe  = 0;
            decimal totalHaber = 0;

            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;

            var comprobantes = await _context.Comprobantes
                                             .Include(c => c.DetalleComprobantes)
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .OrderByDescending(e => e.Serie)
                                             .ToListAsync();

            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            List<ComprobanteConDetalles> comprobantesConDetalles =
                new List<ComprobanteConDetalles>();
            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var Nperiodo = p.Nombre;

                    if (IdPeriodo == 0 || p.IdPeriodo == IdPeriodo)
                    {
                        var NGestion = gestion.Nombre;
                        foreach (var c in comprobantes)
                        {
                            var NMoneda  = await GetMonedaName(c.IdMoneda);
                            var NEmpresa = await GetEmpresaName(c.IdEmpresa);
                            if (c.Estado == EstadoComprobante.Abierto &&
                                c.Fecha  >= p.FechaInicio             &&
                                c.Fecha  <= p.FechaFin)
                            {
                                foreach (var detalle in c.DetalleComprobantes)
                                {
                                    var cuenta =
                                        await _context.Cuentas.FindAsync(
                                            detalle.IdCuenta);
                                    totalDebe  += detalle.MontoDebe;
                                    totalHaber += detalle.MontoHaber;
                                    LibroDiarioReporte libroDiario =
                                        new LibroDiarioReporte
                                        {
                                            NombreGestion = NGestion,
                                            NombreEmpresa = NEmpresa,
                                            NombrePeriodo = IdPeriodo == 0
                                                ? "Todos"
                                                : Nperiodo,
                                            NombreMoneda = NMoneda,
                                            Debe         = detalle.MontoDebe,
                                            Haber        = detalle.MontoHaber,
                                            NombreCuenta = detalle.NombreCuenta,
                                            Fecha        = c.Fecha,
                                            Glosa        = detalle.Glosa,
                                        };

                                    var comprobante =
                                        comprobantesConDetalles.FirstOrDefault(x =>
                                            x.IdComprobante == c.IdComprobante);
                                    if (comprobante == null)
                                    {
                                        comprobante = new ComprobanteConDetalles
                                        {
                                            IdComprobante = c.IdComprobante,
                                            Detalles      = new List<LibroDiarioReporte>()
                                        };
                                        comprobantesConDetalles.Add(comprobante);
                                    }

                                    comprobante.Detalles.Add(libroDiario);
                                }
                            }
                        }
                    }
                }
            }

            if (comprobantesConDetalles != null && comprobantesConDetalles.Count > 0)
            {
                return Ok(new ResultadoLibroDiario
                {
                    Comprobantes = comprobantesConDetalles,
                    TotalDebe    = totalDebe,
                    TotalHaber   = totalHaber
                });
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("LibroDiarioAlternativo/{IdGestion}/{IdPeriodo}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> LibroDiarioAlternativo(
        [FromRoute] int IdGestion, [FromRoute] int? IdPeriodo)
    {
        try
        {
            decimal totalDebe  = 0;
            decimal totalHaber = 0;

            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;

            var comprobantes = await _context.Comprobantes
                                             .Include(c => c.DetalleComprobantes)
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .OrderByDescending(e => e.Serie)
                                             .ToListAsync();

            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            var comprobantesConDetalles =
                new List<ComprobanteConDetalles>();
            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var Nperiodo = p.Nombre;

                    if (IdPeriodo == 0 || p.IdPeriodo == IdPeriodo)
                    {
                        var NGestion = gestion.Nombre;
                        foreach (var c in comprobantes)
                        {
                            var NMoneda  = await GetMonedaName(c.IdMoneda);
                            var NEmpresa = await GetEmpresaName(c.IdEmpresa);
                            if (c.Estado == EstadoComprobante.Abierto &&
                                c.Fecha  >= p.FechaInicio             &&
                                c.Fecha  <= p.FechaFin)
                            {
                                foreach (var detalle in c.DetalleComprobantes)
                                {
                                    var cuenta =
                                        await _context.Cuentas.FindAsync(
                                            detalle.IdCuenta);
                                    totalDebe  += detalle.MontoDebeAlt;
                                    totalHaber += detalle.MontoHaberAlt;
                                    LibroDiarioReporte libroDiario =
                                        new LibroDiarioReporte
                                        {
                                            NombreGestion = NGestion,
                                            NombreEmpresa = NEmpresa,
                                            NombrePeriodo = IdPeriodo == 0
                                                ? "Todos"
                                                : Nperiodo,
                                            NombreMoneda = NMoneda,
                                            Debe         = detalle.MontoDebeAlt,
                                            Haber        = detalle.MontoHaberAlt,
                                            NombreCuenta = detalle.NombreCuenta,
                                            Fecha        = c.Fecha,
                                            Glosa        = detalle.Glosa,
                                        };

                                    var comprobante =
                                        comprobantesConDetalles.FirstOrDefault(x =>
                                            x.IdComprobante == c.IdComprobante);
                                    if (comprobante == null)
                                    {
                                        comprobante = new ComprobanteConDetalles
                                        {
                                            IdComprobante = c.IdComprobante,
                                            Detalles      = new List<LibroDiarioReporte>()
                                        };
                                        comprobantesConDetalles.Add(comprobante);
                                    }

                                    comprobante.Detalles.Add(libroDiario);
                                }
                            }
                        }
                    }
                }
            }

            if (comprobantesConDetalles != null && comprobantesConDetalles.Count > 0)
            {
                return Ok(new ResultadoLibroDiario
                {
                    Comprobantes = comprobantesConDetalles,
                    TotalDebe    = totalDebe,
                    TotalHaber   = totalHaber
                });
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("StockCategorias/{IdCategoria}/{Cantidad}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> StockCategorias([FromRoute] int IdCategoria,
                                                     [FromRoute] int Cantidad)
    {
        try
        {
            var categorias = await GetAllChildCategories(IdCategoria);
            var articulos = await _context.Articulo
                                          .Include(x => x.ArticuloCategorias)
                                          .Where(a => a.Cantidad <= Cantidad)
                                          .ToListAsync();

            List<StockReporte> reporte = new List<StockReporte>();

            foreach (var articulo in articulos)
            {
                foreach (var articuloCategoria in articulo.ArticuloCategorias)
                {
                    if (categorias.Contains(articuloCategoria.IdCategoria))
                    {
                        reporte.Add(new StockReporte
                        {
                            NombreArticulo = articulo.Nombre,
                            StockArticulo  = articulo.Cantidad,
                            Precio = articulo.PrecioVenta
                            // Estado = articulo.Estado,
                            // propiedades del StockReporte
                        });
                    }
                }
            }

            return Ok(reporte);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("LibroMayorPrincipal/{IdGestion}/{IdPeriodo}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> LibroMayorPrincipal(
        [FromRoute] int IdGestion, [FromRoute] int? IdPeriodo)
    {
        try
        {
            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;

            var comprobantes = await _context.Comprobantes
                                             .Include(c => c.DetalleComprobantes)
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .ToListAsync();

            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            var saldosPorCuenta = new Dictionary<int, decimal>();
            var comprobantesConDetalles =
                new List<ComprobanteMayorConDetalles>();
            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var Nperiodo = p.Nombre;

                    if (IdPeriodo == 0 || p.IdPeriodo == IdPeriodo)
                    {
                        var NGestion = gestion.Nombre;
                        foreach (var c in comprobantes)
                        {
                            var NMoneda  = await GetMonedaName(c.IdMoneda);
                            var NEmpresa = await GetEmpresaName(c.IdEmpresa);
                            if (c.Estado == EstadoComprobante.Abierto &&
                                c.Fecha  >= p.FechaInicio             &&
                                c.Fecha  <= p.FechaFin)
                            {
                                var cuentas =
                                    await _context.Cuentas.ToDictionaryAsync(c =>
                                        c.IdCuenta);

                                foreach (var detalle in c.DetalleComprobantes)
                                {
                                    if (!cuentas.TryGetValue((int)detalle.IdCuenta,
                                            out var cuenta))
                                    {
                                        continue;
                                    }

                                    var cu = cuenta.IdCuenta;
                                    var comprobante =
                                        comprobantesConDetalles.FirstOrDefault(x =>
                                            x.IdComprobante == cu);
                                    if (comprobante == null)
                                    {
                                        comprobante = new ComprobanteMayorConDetalles
                                        {
                                            IdComprobante = cu,
                                            Detalles      = new List<LibroMayorReporte>()
                                        };
                                        comprobantesConDetalles.Add(comprobante);
                                    }

                                    if (!saldosPorCuenta.ContainsKey(cu))
                                    {
                                        saldosPorCuenta[cu] = 0;
                                    }

                                    if (detalle.MontoDebe > 0)
                                    {
                                        saldosPorCuenta[cu] += detalle.MontoDebe;
                                    }
                                    else
                                    {
                                        saldosPorCuenta[cu] -= detalle.MontoHaber;
                                    }

                                    var libromayor = new LibroMayorReporte
                                    {
                                        NombreGestion = NGestion,
                                        NombreEmpresa = NEmpresa,
                                        NombrePeriodo =
                                            IdPeriodo == 0 ? "Todos" : Nperiodo,
                                        NombreMoneda = NMoneda,
                                        Debe         = detalle.MontoDebe,
                                        Haber        = detalle.MontoHaber,
                                        NombreCuenta = detalle.NombreCuenta,
                                        Fecha        = c.Fecha,
                                        Glosa        = detalle.Glosa,
                                        Nro          = c.Serie,
                                        Tipo         = c.TipoComprobante.GetDisplayName(),
                                        Saldo        = saldosPorCuenta[cu],
                                    };

                                    comprobante.Detalles.Add(libromayor);
                                }
                            }
                        }
                    }
                }
            }

            if (comprobantesConDetalles != null && comprobantesConDetalles.Count > 0)
            {
                return Ok(new ResultadoLibroMayor
                {
                    Comprobantes = comprobantesConDetalles,
                });
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("LibroMayorAlt/{IdGestion}/{IdPeriodo}")]
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> LibroMayorAlt(
        [FromRoute] int IdGestion, [FromRoute] int? IdPeriodo)
    {
        try
        {
            var gestiones = await _context.Gestiones.Include(g => g.Periodos)
                                          .Where(x => x.IdGestion == IdGestion)
                                          .ToListAsync();
            var IdEmpresa = gestiones[0].IdEmpresa;

            var comprobantes = await _context.Comprobantes
                                             .Include(c => c.DetalleComprobantes)
                                             .Where(x => x.IdEmpresa == IdEmpresa)
                                             .ToListAsync();

            if (comprobantes.Count == 0)
            {
                return StatusCode(404);
            }

            var saldosPorCuenta = new Dictionary<int, decimal>();
            var comprobantesConDetalles =
                new List<ComprobanteMayorConDetalles>();
            foreach (var gestion in gestiones)
            {
                foreach (var p in gestion.Periodos)
                {
                    var Nperiodo = p.Nombre;

                    if (IdPeriodo == 0 || p.IdPeriodo == IdPeriodo)
                    {
                        var NGestion = gestion.Nombre;
                        foreach (var c in comprobantes)
                        {
                            var NMoneda  = await GetMonedaName(c.IdMoneda);
                            var NEmpresa = await GetEmpresaName(c.IdEmpresa);
                            if (c.Estado == EstadoComprobante.Abierto &&
                                c.Fecha  >= p.FechaInicio             &&
                                c.Fecha  <= p.FechaFin)
                            {
                                var cuentas =
                                    await _context.Cuentas.ToDictionaryAsync(c =>
                                        c.IdCuenta);

                                foreach (var detalle in c.DetalleComprobantes)
                                {
                                    if (!cuentas.TryGetValue(detalle.IdCuenta,
                                            out var cuenta))
                                    {
                                        continue;
                                    }

                                    var cu = cuenta.IdCuenta;
                                    var comprobante =
                                        comprobantesConDetalles.FirstOrDefault(x =>
                                            x.IdComprobante == cu);
                                    if (comprobante == null)
                                    {
                                        comprobante = new ComprobanteMayorConDetalles
                                        {
                                            IdComprobante = cu,
                                            Detalles      = new List<LibroMayorReporte>()
                                        };
                                        comprobantesConDetalles.Add(comprobante);
                                    }

                                    if (!saldosPorCuenta.ContainsKey(cu))
                                    {
                                        saldosPorCuenta[cu] = 0;
                                    }

                                    if (detalle.MontoHaberAlt > 0)
                                    {
                                        saldosPorCuenta[cu] += detalle.MontoDebeAlt;
                                    }
                                    else
                                    {
                                        saldosPorCuenta[cu] -= detalle.MontoHaberAlt;
                                    }

                                    var libromayor = new LibroMayorReporte
                                    {
                                        NombreGestion = NGestion,
                                        NombreEmpresa = NEmpresa,
                                        NombrePeriodo =
                                            IdPeriodo == 0 ? "Todos" : Nperiodo,
                                        NombreMoneda = NMoneda,
                                        Debe         = detalle.MontoDebeAlt,
                                        Haber        = detalle.MontoHaberAlt,
                                        NombreCuenta = detalle.NombreCuenta,
                                        Fecha        = c.Fecha,
                                        Glosa        = detalle.Glosa,
                                        Nro          = c.Serie,
                                        Tipo         = c.TipoComprobante.GetDisplayName(),
                                        Saldo        = saldosPorCuenta[cu],
                                    };

                                    comprobante.Detalles.Add(libromayor);
                                }
                            }
                        }
                    }
                }
            }

            if (comprobantesConDetalles != null && comprobantesConDetalles.Count > 0)
            {
                return Ok(new ResultadoLibroMayor
                {
                    Comprobantes = comprobantesConDetalles,
                });
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }



    private async Task<List<int>> GetAllChildCategories(int IdCategoria)
    {
        var allCategories = new List<int> { IdCategoria };
        var childCategories = await _context.Categoria
                                            .Where(x => x.IdCategoriaPadre == IdCategoria)
                                            .ToListAsync();

        foreach (var child in childCategories)
        {
            allCategories.AddRange(await GetAllChildCategories(child.IdCategoria));
        }

        return allCategories;
    }


    private async Task<string> GetEmpresaName(int idEmpresa) =>
        await Task.FromResult(_context.Empresas
                                      .SingleAsync(e =>
                                          e.IdEmpresa == idEmpresa)
                                      .Result.Nombre);

    private async Task<string> GetMonedaName(int idMoneda) =>
        await Task.FromResult(_context.Monedas.SingleAsync(m =>
            m.IdMoneda == idMoneda).Result.Nombre);

    private async Task<string> GetAritculoName(int idArticulo) =>
        await Task.FromResult(_context.Articulo
                                      .FirstOrDefaultAsync(a =>
                                          a.IdArticulo == idArticulo)
                                      .Result!.Nombre ?? string.Empty);
}