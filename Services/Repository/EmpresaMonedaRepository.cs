using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class EmpresaMonedaRepository : IEmpresaMonedaRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public EmpresaMonedaRepository(ApplicationDbContext applicationDbContext,
                                   IMapper              mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<List<EmpresaMonedaDto>> GetEmpresasMonedas(int idEmpresa)
    {
        var listaEmpresaMoneda = await _applicationDbContext.EmpresaMonedas
                                                            .AsNoTracking()
                                                            .Where(em => em.IdEmpresa == idEmpresa)
                                                            .ToListAsync();
        return await Task.FromResult(
            _mapper.Map<List<EmpresaMonedaDto>>(listaEmpresaMoneda));
    }

    public Task<EmpresaMonedaDto> GetEmpresaMoneda(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<EmpresaMonedaDto> CreateEmpresaMoneda(
        EmpresaMonedaDto empresaMonedaDto, int idEmpresa, int idMoneda)
    {
        var empresaMonedaDb =
            _mapper.Map<EmpresaMonedaDto, Modelos.Models.EmpresaMoneda>(empresaMonedaDto);
        empresaMonedaDb.IdEmpresa         = idEmpresa;
        empresaMonedaDb.IdMonedaPrincipal = idMoneda;

        await _applicationDbContext.EmpresaMonedas.AddAsync(empresaMonedaDb);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<Modelos.Models.EmpresaMoneda, EmpresaMonedaDto>(empresaMonedaDb));
    }

    public async Task<EmpresaMonedaDto> CrearMonedaAlternativa(EmpresaMonedaDto empresaMonedaDto,
                                                               int              idEmpresa,
                                                               int              idMonedaAlterna,
                                                               int              idMonedaPrincipal)
    {
        var empresaMonedaDb =
            _mapper.Map<EmpresaMonedaDto, Modelos.Models.EmpresaMoneda>(empresaMonedaDto);
        empresaMonedaDb.IdEmpresa           = idEmpresa;
        empresaMonedaDb.IdMonedaPrincipal   = idMonedaPrincipal;
        empresaMonedaDb.IdMonedaAlternativa = idMonedaAlterna;

        var listaEmpresaMonedas =
            _applicationDbContext.EmpresaMonedas.Where(em => em.IdEmpresa == idEmpresa);
        await listaEmpresaMonedas.Skip(1).ForEachAsync(em =>
            em.Estado = EstadoEmpresaMoneda.Cerrado);

        await _applicationDbContext.EmpresaMonedas.AddAsync(empresaMonedaDb);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<Modelos.Models.EmpresaMoneda, EmpresaMonedaDto>(empresaMonedaDb));
    }

    public async Task<EmpresaMonedaDto?> UpdateMoneda(
        JsonPatchDocument<EmpresaMonedaDto> patchDoc, int id)
    {
        var empresaMonedaDb = await _applicationDbContext.EmpresaMonedas.FindAsync(id);

        if (empresaMonedaDb is not null)
        {
            var empresaMonedaDto =
                _mapper.Map<Modelos.Models.EmpresaMoneda, EmpresaMonedaDto>(
                    empresaMonedaDb);
            patchDoc.ApplyTo(empresaMonedaDto);

            _mapper.Map(empresaMonedaDto, empresaMonedaDb);
        }

        await _applicationDbContext.SaveChangesAsync();

        return _mapper.Map<Modelos.Models.EmpresaMoneda, EmpresaMonedaDto>(
            empresaMonedaDb);
    }

    public async Task<List<EmpresaMonedaDto>> GetMonedaAlternativasPerEmpresa(int idEmpresa)
    {
        var empresaMonedasActivas = await _applicationDbContext.EmpresaMonedas.Where(em => em.IdEmpresa == idEmpresa &&
            em.Estado == EstadoEmpresaMoneda.Abierto).ToListAsync();
        return await Task.FromResult(_mapper.Map<List<EmpresaMonedaDto>>(empresaMonedasActivas));
    }
}