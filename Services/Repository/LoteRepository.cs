using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class LoteRepository : ILoteRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public LoteRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<LoteDto> CrearLote(LoteDto dto, int idArticulo)
    {
        var lote = _mapper.Map<LoteDto, Lote>(dto);
        var maxSerie = await _applicationDbContext.Lotes
                                                  .Where(l => l.IdArticulo == idArticulo)
                                                  .MaxAsync(e => e.NroLote);
        lote.NroLote = (maxSerie ?? 0) + 1;
        await _applicationDbContext.AddAsync(lote);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Lote, LoteDto>(lote);
    }

    public async Task<IEnumerable<LoteDto>> GetLotes(int idArticulo)
    {
        var listaLotes = await _applicationDbContext.Lotes
                                                    .Where(l =>
                                                        l.IdArticulo == idArticulo)
                                                    .ToListAsync();
        return _mapper.Map<List<LoteDto>>(listaLotes);
    }

    public async Task<bool> EliminarLote(int idLote, int idArticulo)
    {
        var lotePorArticulo = await _applicationDbContext.Lotes.FirstOrDefaultAsync(l =>
            l.IdArticulo == idArticulo &&
            l.IdLote     == idLote);
        if (lotePorArticulo is null)
        {
            return false;
        }

        lotePorArticulo.EstadoLote = EstadoLote.Anulado;
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }
}