using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
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

    public async Task<object> CreateDetalleComprobante(
        DetalleComprobanteDto detalleComprobanteDto)
    {
        var detalleComprobante =
            _mapper.Map<DetalleComprobanteDto, Modelos.Models.DetalleComprobante>(
                detalleComprobanteDto);
        await _dbContext.AddAsync(detalleComprobante);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Modelos.Models.DetalleComprobante, DetalleComprobanteDto>(
            detalleComprobante);
    }

    public async Task<object> GetComprobantes(int idComprobante)
    {
        var detalles = await _dbContext.DetalleComprobantes.Where(dc =>
            dc.IdComprobante ==
            idComprobante).ToListAsync();
        return await Task.FromResult(_mapper.Map<List<DetalleComprobanteDto>>(detalles));
    }
}