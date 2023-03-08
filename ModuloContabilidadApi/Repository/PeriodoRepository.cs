using AutoMapper;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;

namespace ModuloContabilidadApi.Repository;

public class PeriodoRepository : IPeriodoRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public PeriodoRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public Task<IEnumerable<PeriodoDto>> GetModelos()
    {
        throw new NotImplementedException();
    }

    public Task<PeriodoDto>              GetModelo(int modeloId)
    {
        throw new NotImplementedException();
    }

    public Task<PeriodoDto>              CreateUpdateModelDto(PeriodoDto periodoDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool>                        DeleteModel(int modeloId)
    {
        throw new NotImplementedException();
    }
}