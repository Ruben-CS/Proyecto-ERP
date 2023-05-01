using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class MonedaRepository : IMonedaRepository
{
    private readonly IMapper              _mapper;
    private readonly ApplicationDbContext _dbContext;

    public MonedaRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<IEnumerable<MonedaDto>> GetAllMonedas()
    {
        var listaEmpresas = await _dbContext.Monedas.ToListAsync();
        return _mapper.Map<List<MonedaDto>>(listaEmpresas);
    }

    public async Task<MonedaDto> GetMoneda(int idMoneda)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteMoneda(int idMoneda)
    {
        throw new NotImplementedException();
    }

    public async Task<MonedaDto> CreateMoneda(MonedaDto monedaDto, int idEmpresa)
    {
        throw new NotImplementedException();
    }

    public async Task<MonedaDto> UpdateMoneda(MonedaDto monedaDto, int idMoneda)
    {
        throw new NotImplementedException();
    }
}