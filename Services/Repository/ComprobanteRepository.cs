using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class ComprobanteRepository : IComprobanteRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper              _mapper;


    public ComprobanteRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<ComprobanteDto> CrearComprobante(
        ComprobanteDto comprobanteDto, int idEmpresa)
    {
        var monedas = await _dbContext.Monedas
                                      .Where(m => m.IdMoneda == comprobanteDto.IdMoneda)
                                      .ToListAsync();
        //todo nombre moneda?
        try
        {
            var gestiones = await _dbContext.Gestiones.Include(g => g.Periodos)
                                            .Where(p => p.IdEmpresa == idEmpresa)
                                            .ToListAsync();
            var fechaEnPeriodoAbierto = false;
            foreach (var gestion in gestiones)
            {
                foreach (var periodo in gestion.Periodos)
                {
                    var idGestion = gestion.IdGestion;
                    if (periodo.Estado       == EstadosPeriodo.Abierto &&
                        comprobanteDto.Fecha >= periodo.FechaInicio    &&
                        comprobanteDto.Fecha <= periodo.FechaFin)
                    {
                        if (comprobanteDto.TipoComprobante == TipoComprobante.Apertura)
                        {
                            var gestionActual =
                                await _dbContext.Gestiones.FirstOrDefaultAsync(g =>
                                    g.IdGestion == idGestion);
                            if (gestionActual is not null)
                            {
                                var comprobanteApertura =
                                    await _dbContext.Comprobantes.FirstOrDefaultAsync(c =>
                                        c.IdEmpresa == idEmpresa && c.TipoComprobante ==
                                        TipoComprobante.Apertura);
                                if (comprobanteApertura is not null)
                                {
                                    //todo check this..
                                }
                            }
                        }

                        fechaEnPeriodoAbierto = true;
                        break;
                    }
                }

                if (fechaEnPeriodoAbierto) break;
            }

            if (!fechaEnPeriodoAbierto)
            {
                //throw an error
            }

            var comprobante = _mapper.Map<ComprobanteDto, Comprobante>(comprobanteDto);
            await _dbContext.AddAsync(comprobante);
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(
                _mapper.Map<Comprobante, ComprobanteDto>(comprobante));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    Task<IEnumerable<ComprobanteDto>> IComprobanteRepository.GetAllComprobantes(
        int idEmpresa)
    {
        throw new NotImplementedException();
    }

    public Task<ComprobanteDto> GetCombrobanteById(int idComprobante)
    {
        throw new NotImplementedException();
    }

    public Task<ComprobanteDto> EditComprobante(int idComprobante)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteComprobante(int idComprobante)
    {
        throw new NotImplementedException();
    }
}