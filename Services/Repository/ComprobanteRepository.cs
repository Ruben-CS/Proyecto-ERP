using AutoMapper;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
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
        var comprobante = _mapper.Map<ComprobanteDto, Comprobante>(comprobanteDto);
        await _dbContext.AddAsync(comprobante);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Comprobante, ComprobanteDto>(comprobante);
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