using AutoMapper;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class DetalleComprobanteRepository : IDetalleComprobanteRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper              _mapper;

    public DetalleComprobanteRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<object> CreateDetalleComprobante(int idComprobante,
                                                 DetalleComprobanteDto detalleComprobanteDto)
    {
        var detalleComprobante = _mapper.Map<DetalleComprobanteDto, DetalleComprobante>(detalleComprobanteDto);
        await _dbContext.AddAsync(detalleComprobante);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<DetalleComprobante, DetalleComprobanteDto>(detalleComprobante);
    }
}