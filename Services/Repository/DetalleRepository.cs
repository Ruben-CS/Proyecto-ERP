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

        var articulo = await _applicationDbContext.Articulo.SingleAsync(a =>
            a.IdArticulo == dto.IdArticulo);

        articulo.Cantidad -= dto.Cantidad;
        _applicationDbContext.Entry(articulo).State = EntityState.Modified; // <-- Mark as Modified

        var lotes = await _applicationDbContext.Lotes.Where(
            l => l.NroLote    == dto.NroLote &&
                 l.IdArticulo == dto.IdArticulo).ToListAsync();

        var lote = lotes.First();
        lote.Stock -= dto.Cantidad;

        if (lote.Stock < 1)
            lote.EstadoLote = EstadoLote.Agotado;

        _applicationDbContext.Entry(lote).State = EntityState.Modified; // <-- Mark as Modified

        await _applicationDbContext.AddAsync(detalleVenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Detalle, DetalleDto>(detalleVenta);
     }

    public Task<bool> EliminarDetalleVenta(DetalleDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DetalleDto>> ListarDetalles(int idNota)
    {
        var detalleVenta = await _applicationDbContext.Detalle
                                                      .AsNoTracking()
                                                      .Where(d => d.IdNota == idNota)
                                                      .Select(d =>
                                                          _mapper.Map<DetalleDto>(d))
                                                      .ToListAsync();
        return detalleVenta;
    }
}