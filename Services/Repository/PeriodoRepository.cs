using AutoMapper;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

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