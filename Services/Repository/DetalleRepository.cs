using AutoMapper;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class DetalleRepository : IDetalleRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;


    public DetalleRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<DetalleDto> AgregarDetalleVenta(DetalleDto dto)
    {
        var detalleVenta = _mapper.Map<DetalleDto, Detalle>(dto);
        await _applicationDbContext.AddAsync(detalleVenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Detalle, DetalleDto>(detalleVenta);
    }

    public Task<bool> EliminarDetalleVenta(DetalleDto dto)
    {
        throw new NotImplementedException();
    }
}