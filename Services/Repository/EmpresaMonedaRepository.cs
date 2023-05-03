using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
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
        var listaEmpresaMoneda =
            await _applicationDbContext.EmpresaMonedas.Where(em =>
                em.IdEmpresa == idEmpresa).ToListAsync();
        return _mapper.Map<List<EmpresaMonedaDto>>(listaEmpresaMoneda);
    }

    public Task<EmpresaMonedaDto> GetEmpresaMoneda(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<EmpresaMonedaDto> CreateEmpresaMoneda(
        EmpresaMonedaDto empresaMonedaDto)
    {
        var empresaMonedaDb =
            _mapper.Map<EmpresaMonedaDto, Modelos.Models.EmpresaMoneda>(empresaMonedaDto);
        _applicationDbContext.EmpresaMonedas.Add(empresaMonedaDb);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<Modelos.Models.EmpresaMoneda, EmpresaMonedaDto>(empresaMonedaDb));
    }
}