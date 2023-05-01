using AutoMapper;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

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

    public Task<List<EmpresaMonedaDto>> GetEmpresasMonedas()
    {
        throw new NotImplementedException();
    }

    public Task<EmpresaMonedaDto> GetEmpresaMoneda(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<EmpresaMonedaDto> CreateEmpresaMoneda(
        EmpresaMonedaDto empresaMonedaDto)
    {
        var empresaMonedaDb =
            _mapper.Map<EmpresaMonedaDto, EmpresaMoneda>(empresaMonedaDto);
        _applicationDbContext.EmpresaMonedas.Add(empresaMonedaDb);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<EmpresaMoneda, EmpresaMonedaDto>(empresaMonedaDb));
    }
}