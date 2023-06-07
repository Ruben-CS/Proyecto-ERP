using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
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
        await _applicationDbContext.AddAsync(lote);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Lote, LoteDto>(lote);
    }

    public async Task<IEnumerable<LoteDto>> GetLotes(int idNota)
    {
        var listaLotes = await _applicationDbContext.Lotes
                                                    .Where(l =>
                                                        l.IdNota == idNota)
                                                    .ToListAsync();
        return _mapper.Map<List<LoteDto>>(listaLotes);
    }

    public async Task<IEnumerable<LoteDto>> GetLotesPorArticulo(int idArticulo)
    {
        var listaLotes = await _applicationDbContext.Lotes.Where(l => l.IdArticulo ==
            idArticulo).ToListAsync();
        return _mapper.Map<List<LoteDto>>(listaLotes);
    }
}