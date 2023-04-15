using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class PeriodoRepository : IPeriodoRepository
{
    private readonly _applicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public PeriodoRepository(_applicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<IEnumerable<PeriodoDto>> GetModelos(int gestionId)
    {
        var listaPeriodos =
            await _applicationDbContext.Periodos.Where(periodo =>
                periodo.IdGestion == gestionId).ToListAsync();

        return _mapper.Map<List<PeriodoDto>>(listaPeriodos);
    }

    public async Task<PeriodoDto> GetModelo(int modeloId)
    {
        var periodo =
            await _applicationDbContext.Periodos.SingleOrDefaultAsync(periodo =>
                periodo.IdPeriodo == modeloId);
        return await Task.FromResult(_mapper.Map<PeriodoDto>(periodo));
    }

    public async Task<PeriodoDto> CreateUpdateModelDto(
        PeriodoDto periodoDto, int idGestion)
    {
        var periodo = _mapper.Map<PeriodoDto, Modelos.Models.Periodo>(periodoDto);
        try
        {
            await _applicationDbContext.AddAsync(periodo);
            await _applicationDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(_mapper.Map<Modelos.Models.Periodo, PeriodoDto>(periodo));
    }


    public async Task<bool> DeleteModel(int modeloId)
    {
        try
        {
            var periodo =
                await _applicationDbContext.Periodos.FirstOrDefaultAsync(periodo =>
                    periodo.IdPeriodo == modeloId);

            if (periodo is null)
            {
                return await Task.FromResult(false);
            }

            _applicationDbContext.Remove(periodo);
            await _applicationDbContext.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PeriodoDto> UpdateModel(PeriodoDto periodoDto, int idGestion, int idPeriodo)
    {
        var periodo = await _applicationDbContext.Periodos.SingleAsync(periodo =>
            periodo.IdPeriodo == idPeriodo && periodo.IdGestion == idGestion);

        if (periodo is null)
        {
            throw new NullReferenceException("periodo no existe");
        }

        _mapper.Map(periodoDto, periodo);
        _applicationDbContext.Entry(periodo).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(_mapper.Map<PeriodoDto>(periodo));
    }
}