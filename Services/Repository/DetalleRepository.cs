using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
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

        var aritculo = await _applicationDbContext.Articulo.SingleAsync(a =>
            a.IdArticulo == dto.IdArticulo);

        aritculo.Cantidad -= dto.Cantidad;

        var lotes = await _applicationDbContext.Lotes.Where(
            l => l.NroLote    == dto.NroLote &&
                 l.IdArticulo == dto.IdArticulo).ToListAsync();

        lotes.First().Stock -= dto.Cantidad;

        if (lotes.First().Stock < 1)
            lotes.First().EstadoLote = EstadoLote.Agotado;

        await _applicationDbContext.AddAsync(detalleVenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Detalle, DetalleDto>(detalleVenta);
    }

    public Task<bool> EliminarDetalleVenta(DetalleDto dto)
    {
        throw new NotImplementedException();
    }
}